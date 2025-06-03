using BackendCRUD.ApiService.Models;

namespace BackendCRUD.ApiService.Services.Interfaces
{
    public interface ICVService
    {
        List<Curriculum> GetCVs(List<int> ids);
        Curriculum GetCV(int id);
        void AddCV(Curriculum cv);
        bool DeleteCV(int id);
    }
}
