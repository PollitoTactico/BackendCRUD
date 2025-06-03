using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.DTOs.Comparation;
using System.Text;
using System.Text.RegularExpressions;
using BackendCRUD.ApiService.Extensions;
using BackendCRUD.ApiService.Services.Interfaces;
using System.Globalization;
using BackendCRUD.ApiService.DTos.Comparation;

public class KeywordProcessor : IKeywordProcessor
{
    private readonly List<string> _stopWords = new List<string>
    {
        "el", "la", "los", "las", "un", "una", "unos", "unas",
        "de", "del", "al", "y", "o", "pero", "con", "para", "por",
        "como", "en", "a", "su", "sus", "se", "que", "es", "son"
    };

    public List<KeywordWeightDTO> ExtractKeywords(ProfileUser profile)
    {
        var keywords = new List<KeywordWeightDTO>();

        // Extraer keywords de cada campo con pesos diferentes
        if (!string.IsNullOrEmpty(profile.MisionCargo))
            keywords.AddRange(ProcessText(profile.MisionCargo, baseWeight: 10));

        if (!string.IsNullOrEmpty(profile.ConocimientosCargo))
            keywords.AddRange(ProcessText(profile.ConocimientosCargo, baseWeight: 8));

        if (!string.IsNullOrEmpty(profile.ConocimientoTecnologico))
            keywords.AddRange(ProcessText(profile.ConocimientoTecnologico, baseWeight: 12));

        if (!string.IsNullOrEmpty(profile.FormacionAcademica))
            keywords.AddRange(ProcessText(profile.FormacionAcademica, baseWeight: 6));

        // Consolidar keywords y sumar pesos
        return ConsolidateKeywords(keywords);
    }

    private List<KeywordWeightDTO> ProcessText(string text, int baseWeight)
    {
        // Normalización avanzada del texto
        var normalizedText = NormalizeText(text);

        // Tokenización mejorada
        var words = Regex.Split(normalizedText, @"\W+")
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word => word.Length > 3)
            .Where(word => !_stopWords.Contains(word.ToLower()));

        // Agrupar palabras similares (ej: "desarrollo", "desarrollador")
        var stemmedWords = words
            .Select(word => new
            {
                Original = word,
                Stemmed = word.StemWord() // Asumiendo que tienes un método de stemming
            })
            .GroupBy(x => x.Stemmed);

        return stemmedWords.Select(group => new KeywordWeightDTO
        {
            Keyword = group.Key,
            Weight = baseWeight * group.Count() // Peso basado en frecuencia
        }).ToList();
    }

    private string NormalizeText(string text)
    {
        // Eliminar acentos y caracteres especiales
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString()
            .ToLower()
            .Replace("\n", " ")
            .Replace("\r", " ");
    }

    private List<KeywordWeightDTO> ConsolidateKeywords(List<KeywordWeightDTO> keywords)
    {
        return keywords
            .GroupBy(k => k.Keyword)
            .Select(g => new KeywordWeightDTO
            {
                Keyword = g.Key,
                Weight = g.Sum(x => x.Weight) +
                        (g.Key.Length > 7 ? 2 : 0) + // Bonus por palabras largas
                        (IsTechnicalTerm(g.Key) ? 5 : 0) // Bonus por términos técnicos
            })
            .OrderByDescending(k => k.Weight)
            .Take(20) // Limitar a las 20 palabras más relevantes
            .ToList();
    }

    private bool IsTechnicalTerm(string word)
    {
        var technicalTerms = new List<string>
        {
            "programación", "desarrollo", "backend", "frontend", "fullstack",
            "csharp", "java", "python", "javascript", "sql", "nosql",
            "entity", "framework", "api", "microservicios", "docker",
            "kubernetes", "azure", "aws", "cloud", "devops"
        };

        return technicalTerms.Contains(word.ToLower());
    }

    List<BackendCRUD.ApiService.DTos.Comparation.KeywordWeightDTO> IKeywordProcessor.ExtractKeywords(ProfileUser profile)
    {
        var keywords = new List<KeywordWeightDTO>();

        // Extraer keywords de cada campo con pesos diferentes
        if (!string.IsNullOrEmpty(profile.MisionCargo))
            keywords.AddRange(ProcessText(profile.MisionCargo, baseWeight: 10));

        if (!string.IsNullOrEmpty(profile.ConocimientosCargo))
            keywords.AddRange(ProcessText(profile.ConocimientosCargo, baseWeight: 8));

        if (!string.IsNullOrEmpty(profile.ConocimientoTecnologico))
            keywords.AddRange(ProcessText(profile.ConocimientoTecnologico, baseWeight: 12));

        if (!string.IsNullOrEmpty(profile.FormacionAcademica))
            keywords.AddRange(ProcessText(profile.FormacionAcademica, baseWeight: 6));

        // Consolidar keywords y sumar pesos
        return ConsolidateKeywords(keywords);
        
    }
}