using BackendCRUD.ApiService.DTos.Comparation;
using BackendCRUD.ApiService.DTOs;
namespace BackendCRUD.ApiService.DTOs.Comparation
{
    public class ComparationResponseDTO
    {
        public string ProfileName { get; set; }
        public List<ComparationResultDTO> Results { get; set; }
    }

    public class ComparationResultDTO
    {
        public int CvId { get; set; }
        public string CvName { get; set; }
        public double MatchPercentage { get; set; }
        public List<KeywordWeightDTO> MatchedKeywords { get; set; }
        public List<KeywordWeightDTO> ProfileKeywords { get; set; }
        public List<KeywordWeightDTO> FrequentWords { get; set; }
    }
}