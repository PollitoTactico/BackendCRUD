using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BackendCRUD.ApiService.Services.Implementations
{
    public class PdfServiceProxy : IPdfService
    {
        private readonly PdfService _realPdfService;
        private readonly ILogger<PdfServiceProxy> _logger;
        private readonly Dictionary<int, Curriculum?> _cache = new();

        // Constructor que recibe el servicio real y un logger para el Proxy
        public PdfServiceProxy(PdfService realPdfService, ILogger<PdfServiceProxy> logger)
        {
            _realPdfService = realPdfService;
            _logger = logger;
        }

        // Método para guardar un PDF, agregamos log de actividad
        public async Task<int> SavePdf(byte[] pdfData, string nombreCandidato)
        {
            _logger.LogInformation($"Guardando PDF para el candidato {nombreCandidato}");
            var result = await _realPdfService.SavePdf(pdfData, nombreCandidato);
            _logger.LogInformation($"PDF guardado con ID {result} para el candidato {nombreCandidato}");
            return result;
        }

        // Método para obtener un PDF, con cacheo simple
        public async Task<Curriculum?> GetPdf(int id)
        {
            if (_cache.ContainsKey(id))
            {
                _logger.LogInformation($"Cache hit para el PDF con ID {id}");
                return _cache[id];
            }

            _logger.LogInformation($"Cache miss para el PDF con ID {id}, obteniendo desde el servicio real");
            var curriculum = await _realPdfService.GetPdf(id);

            // Guardamos el PDF en cache
            if (curriculum != null)
            {
                _cache[id] = curriculum;
            }

            return curriculum;
        }
    }
}
