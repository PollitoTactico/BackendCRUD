using BackendCRUD.ApiService.Data;
using BackendCRUD.ApiService.DTos.Comparation;
using BackendCRUD.ApiService.DTOs.Comparation;
using BackendCRUD.ApiService.Extensions;
using BackendCRUD.ApiService.Services.Interfaces;
using BackendCRUD.ApiService.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BackendCRUD.ApiService.Services.Implementations
{
    public class ComparationService : IComparationService
    {
        private static ComparationService _instance;
        private static readonly object _lock = new object();  // Asegura que solo se cree una instancia
        private readonly ProfileUserRepository _profileUserRepository;
        private readonly IPdfTextExtractor _pdfTextExtractor;
        private readonly IKeywordProcessor _keywordProcessor;
        private readonly ICVService _cvService;
        private readonly ILogger<ComparationService> _logger;

        // Constructor privado para evitar instanciación fuera de la clase
        private ComparationService(
            ProfileUserRepository profileUserRepository,  // Repositorio para obtener perfiles
            IPdfTextExtractor pdfTextExtractor,
            IKeywordProcessor keywordProcessor,
            ICVService cvService,
            ILogger<ComparationService> logger)
        {
            _profileUserRepository = profileUserRepository;
            _pdfTextExtractor = pdfTextExtractor;
            _keywordProcessor = keywordProcessor;
            _cvService = cvService;
            _logger = logger;
        }

        // Método para obtener la instancia única
        public static ComparationService Instance(
            ProfileUserRepository profileUserRepository,
            IPdfTextExtractor pdfTextExtractor,
            IKeywordProcessor keywordProcessor,
            ICVService cvService,
            ILogger<ComparationService> logger)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new ComparationService(profileUserRepository, pdfTextExtractor, keywordProcessor, cvService, logger);
                }
            }
            return _instance;
        }

        public async Task<ComparationResponseDTO> CompareProfileWithCVs(ComparationRequestDTO request)
        {
            try
            {
                var profile = await _profileUserRepository.GetProfileByIdAsync(request.ProfileId);

                if (profile == null)
                {
                    _logger.LogWarning("Perfil con ID {ProfileId} no encontrado", request.ProfileId);
                    throw new KeyNotFoundException($"Perfil con ID {request.ProfileId} no encontrado");
                }

                var weightedKeywords = _keywordProcessor.ExtractKeywords(profile);
                var totalPossibleScore = weightedKeywords.Sum(k => k.Weight);

                var cvs = _cvService.GetCVs(request.CvIds);

                if (!cvs.Any())
                    throw new KeyNotFoundException("No se encontraron CVs para comparar");

                var results = new List<ComparationResultDTO>();

                foreach (var cv in cvs)
                {
                    if (string.IsNullOrEmpty(cv.TextoExtraido))
                        cv.TextoExtraido = await _pdfTextExtractor.ExtractText(cv.PDFCurriculum);

                    var cvText = cv.TextoExtraido.RemoveAccents().ToLower();
                    var matchedKeywords = new List<KeywordWeightDTO>();
                    double totalScore = 0;

                    foreach (var keyword in weightedKeywords)
                    {
                        if (cvText.Contains(keyword.Keyword) || cvText.Contains(keyword.Keyword.StemWord()))
                        {
                            totalScore += keyword.Weight;
                            matchedKeywords.Add(new KeywordWeightDTO
                            {
                                Keyword = keyword.Keyword,
                                Weight = keyword.Weight
                            });
                        }
                    }

                    var percentage = totalPossibleScore > 0 ? (totalScore / totalPossibleScore) * 100 : 0;

                    var frequentWords = GetMostFrequentWords(cvText, 10);

                    results.Add(new ComparationResultDTO
                    {
                        CvId = cv.Id,
                        CvName = cv.NombreCandidato,
                        MatchPercentage = Math.Round(percentage, 2),
                        MatchedKeywords = matchedKeywords.OrderByDescending(k => k.Weight).ToList(),
                        ProfileKeywords = weightedKeywords
                            .Select(k => new KeywordWeightDTO { Keyword = k.Keyword, Weight = k.Weight })
                            .OrderByDescending(k => k.Weight)
                            .ToList(),
                        FrequentWords = frequentWords 
                    });
                }

                return new ComparationResponseDTO
                {
                    ProfileName = profile.NombrePerfil,
                    Results = results.OrderByDescending(r => r.MatchPercentage).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CompareProfileWithCVs");
                throw;
            }
        }

        public async Task<List<KeywordWeightDTO>> GetMostFrequentWordsFromCVs(List<int> cvIds, int top = 10)
        {
            var cvs = _cvService.GetCVs(cvIds);

            if (!cvs.Any())
                throw new KeyNotFoundException("No se encontraron CVs con los IDs proporcionados");

            var combinedText = "";

            foreach (var cv in cvs)
            {
                if (string.IsNullOrEmpty(cv.TextoExtraido))
                    cv.TextoExtraido = await _pdfTextExtractor.ExtractText(cv.PDFCurriculum);

                combinedText += " " + cv.TextoExtraido;
            }

            return GetMostFrequentWords(combinedText, top);
        }

        private List<KeywordWeightDTO> GetMostFrequentWords(string text, int top = 10)
        {
            var wordFrequency = new Dictionary<string, int>();

            var words = text
                .RemoveAccents()
                .ToLower()
                .Split(new[] { ' ', '\n', '\r', '\t', '.', ',', ';', ':', '!', '?', '(', ')', '[', ']', '{', '}', '"' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (word.Length < 4) continue;

                if (wordFrequency.ContainsKey(word))
                    wordFrequency[word]++;
                else
                    wordFrequency[word] = 1;
            }

            return wordFrequency
                .OrderByDescending(kvp => kvp.Value)
                .Take(top)
                .Select(kvp => new KeywordWeightDTO
                {
                    Keyword = kvp.Key,
                    Weight = kvp.Value
                })
                .ToList();
        }
    }
}
