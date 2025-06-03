using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.Models.Comparation;
using BackendCRUD.ApiService.DTos.Comparation;

namespace BackendCRUD.ApiService.Services.Interfaces
{
    public interface IKeywordProcessor
    {
        List<KeywordWeightDTO> ExtractKeywords(ProfileUser profile);
    }
}