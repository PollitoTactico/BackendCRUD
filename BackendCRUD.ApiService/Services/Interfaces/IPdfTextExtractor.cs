namespace BackendCRUD.ApiService.Services.Interfaces
{
    public interface IPdfTextExtractor
    {
        Task<string> ExtractText(byte[] pdfData);
    }
}
