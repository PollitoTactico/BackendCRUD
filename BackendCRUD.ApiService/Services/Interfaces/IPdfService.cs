using BackendCRUD.ApiService.Models;

namespace BackendCRUD.ApiService.Services.Interfaces
{
    public interface IPdfService
    {
        Task<int> SavePdf(byte[] pdfData, string nombreCandidato);
        Task<Curriculum?> GetPdf(int id);
    }
}
