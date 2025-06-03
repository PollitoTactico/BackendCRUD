using BackendCRUD.ApiService.Data;
using BackendCRUD.ApiService.DTos.Comparation;
using BackendCRUD.ApiService.DTOs.Comparation;
using BackendCRUD.ApiService.Extensions;
using BackendCRUD.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BackendCRUD.ApiService.Services.Implementations
{
    public class ComparationService : IComparationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPdfTextExtractor _pdfTextExtractor;
        private readonly IKeywordProcessor _keywordProcessor;
        private readonly ICVService _cvService;
        private readonly ILogger<ComparationService> _logger;

        public ComparationService(
            ApplicationDbContext context,
            IPdfTextExtractor pdfTextExtractor,
            IKeywordProcessor keywordProcessor,
            ICVService cvService,
            ILogger<ComparationService> logger)
        {
            _context = context;
            _pdfTextExtractor = pdfTextExtractor;
            _keywordProcessor = keywordProcessor;
            _cvService = cvService;
            _logger = logger;
        }

        public async Task<ComparationResponseDTO> CompareProfileWithCVs(ComparationRequestDTO request)
        {
            try
            {
                // 1. Obtener el perfil de la base de datos
                var profile = await _context.ProfileUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.id == request.ProfileId);

                if (profile == null)
                {
                    _logger.LogWarning("Perfil con ID {ProfileId} no encontrado", request.ProfileId);
                    throw new KeyNotFoundException($"Perfil con ID {request.ProfileId} no encontrado");
                }

                // 2. Extraer keywords del perfil
                var weightedKeywords = _keywordProcessor.ExtractKeywords(profile);
                var totalPossibleScore = weightedKeywords.Sum(k => k.Weight);

                // 3. Obtener CVs del servicio en memoria
                var cvs = _cvService.GetCVs(request.CvIds);

                if (!cvs.Any())
                    throw new KeyNotFoundException("No se encontraron CVs para comparar");

                var results = new List<ComparationResultDTO>();

                foreach (var cv in cvs)
                {
                    // 4. Extraer texto del PDF si no está extraído
                    if (string.IsNullOrEmpty(cv.TextoExtraido))
                        cv.TextoExtraido = await _pdfTextExtractor.ExtractText(cv.PDFCurriculum);

                    var cvText = cv.TextoExtraido.RemoveAccents().ToLower();
                    var matchedKeywords = new List<KeywordWeightDTO>();
                    double totalScore = 0;

                    // 5. Buscar coincidencias de keywords
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

                    // 6. Calcular el porcentaje de coincidencia
                    var percentage = totalPossibleScore > 0 ? (totalScore / totalPossibleScore) * 100 : 0;

                    // 7. Obtener palabras frecuentes del texto del CV
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
                        FrequentWords = frequentWords // Agregar palabras frecuentes aquí
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

        // Método para obtener las palabras más frecuentes de varios CVs
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