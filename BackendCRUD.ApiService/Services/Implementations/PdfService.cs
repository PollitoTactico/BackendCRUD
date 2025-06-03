using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.Services.Interfaces;

namespace BackendCRUD.ApiService.Services.Implementations
{
    public class PdfService : IPdfService
    {
        private static readonly List<Curriculum> _archivosTemporales = new();

        public Task<int> SavePdf(byte[] pdfData, string nombreCandidato)
        {
            var curriculum = new Curriculum
            {
                Id = _archivosTemporales.Count + 1,
                NombreCandidato = nombreCandidato,
                PDFCurriculum = pdfData,
                FechaSubida = DateTime.Now
            };
            _archivosTemporales.Add(curriculum);
            return Task.FromResult(curriculum.Id);
        }

        public Task<Curriculum?> GetPdf(int id)
        {
            return Task.FromResult(_archivosTemporales.FirstOrDefault(c => c.Id == id));
        }
    }
}
