using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendCRUD.ApiService.Services.Implementations
{
    public class CVServiceProxy : ICVService
    {
        private readonly ICVService _realCVService;
        private readonly ILogger<CVServiceProxy> _logger;
        private readonly Dictionary<int, Curriculum> _cache = new(); // Cache simple para almacenar los CVs.

        public CVServiceProxy(ICVService realCVService, ILogger<CVServiceProxy> logger)
        {
            _realCVService = realCVService;
            _logger = logger;
        }

        // Método para agregar un CV
        public void AddCV(Curriculum curriculum)
        {
            _logger.LogInformation($"Iniciando la carga del CV de {curriculum.NombreCandidato}");

            // Lógica adicional antes de llamar al servicio real
            _realCVService.AddCV(curriculum);

            _logger.LogInformation($"CV de {curriculum.NombreCandidato} cargado correctamente");
        }

        // Método para obtener un CV por su ID
        public Curriculum? GetCV(int id)
        {
            if (_cache.ContainsKey(id))  // Cacheo: verificamos si el CV ya está en el cache
            {
                _logger.LogInformation($"Cache hit para el CV con ID {id}");
                return _cache[id];
            }

            _logger.LogInformation($"Cache miss para el CV con ID {id}, obteniendo desde el servicio real");

            var curriculum = _realCVService.GetCV(id);

            if (curriculum != null)
            {
                // Cacheamos el resultado
                _cache[id] = curriculum;
            }

            return curriculum;
        }

        // Método para obtener varios CVs (implementado correctamente)
        public List<Curriculum> GetCVs(List<int> cvIds)
        {
            var cvList = new List<Curriculum>();

            foreach (var id in cvIds)
            {
                var cv = GetCV(id);
                if (cv != null)
                    cvList.Add(cv);
            }

            return cvList;
        }

        // Método para eliminar un CV
        public bool DeleteCV(int id)
        {
            _logger.LogInformation($"Iniciando la eliminación del CV con ID {id}");

            // Lógica adicional antes de llamar al servicio real
            var result = _realCVService.DeleteCV(id);

            // También eliminamos del cache si existe
            if (_cache.ContainsKey(id))
            {
                _cache.Remove(id);
                _logger.LogInformation($"CV con ID {id} eliminado del cache");
            }

            _logger.LogInformation($"CV con ID {id} eliminado correctamente");

            return result;
        }
    }
}
