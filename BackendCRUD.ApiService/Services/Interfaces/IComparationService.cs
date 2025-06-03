using BackendCRUD.ApiService.DTos.Comparation;
using BackendCRUD.ApiService.DTOs.Comparation;

namespace BackendCRUD.ApiService.Services.Interfaces
{
    public interface IComparationService
    {
        Task<ComparationResponseDTO> CompareProfileWithCVs(ComparationRequestDTO request);
        Task<List<KeywordWeightDTO>> GetMostFrequentWordsFromCVs(List<int> cvIds, int top = 10);

    }
}
