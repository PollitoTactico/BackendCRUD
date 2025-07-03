using BackendCRUD.ApiService.DTOs.Comparation;
using BackendCRUD.ApiService.Repository;  
using BackendCRUD.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendCRUD.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComparationController : ControllerBase
    {
        private readonly IComparationService _comparatationService;
        private readonly ProfileUserRepository _profileUserRepository;  // Repositorio para obtener perfiles
        private readonly ILogger<ComparationController> _logger;

        // Constructor con inyección de dependencias
        public ComparationController(
            IComparationService comparatationService,
            ProfileUserRepository profileUserRepository,
            ILogger<ComparationController> logger)
        {
            _comparatationService = comparatationService;
            _profileUserRepository = profileUserRepository;
            _logger = logger;
        }

        // Endpoint para comparar el perfil con los CVs
        [HttpPost("compare")]
        public async Task<IActionResult> Compare([FromBody] ComparationRequestDTO request)
        {
            try
            {
                _logger.LogInformation("Iniciando comparación para el perfil con ID {ProfileId}", request.ProfileId);

                // Llamada al repositorio para obtener el perfil
                var profile = await _profileUserRepository.GetProfileByIdAsync(request.ProfileId);

                if (profile == null)
                {
                    _logger.LogWarning("Perfil con ID {ProfileId} no encontrado", request.ProfileId);
                    return NotFound($"Perfil con ID {request.ProfileId} no encontrado");
                }

                // Llamada al servicio que orquesta la comparación
                var result = await _comparatationService.CompareProfileWithCVs(request);

                // Retornar los resultados
                _logger.LogInformation("Comparación completada para el perfil con ID {ProfileId}", request.ProfileId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores generales
                _logger.LogError(ex, "Error al comparar el perfil con los CVs");
                return StatusCode(500, $"Error al comparar: {ex.Message}");
            }
        }

        // Endpoint para obtener las palabras más frecuentes de varios CVs
        [HttpPost("frequent-words")]
        public async Task<IActionResult> GetFrequentWords([FromBody] List<int> cvIds)
        {
            try
            {
                _logger.LogInformation("Iniciando la obtención de palabras frecuentes para los CVs con IDs {CvIds}", string.Join(",", cvIds));

                // Llamada al servicio para obtener las palabras más frecuentes
                var result = await _comparatationService.GetMostFrequentWordsFromCVs(cvIds);

                // Retornar los resultados
                _logger.LogInformation("Obtención de palabras frecuentes completada para los CVs con IDs {CvIds}", string.Join(",", cvIds));
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                // Manejo de error cuando no se encuentran los CVs
                _logger.LogWarning("CVs no encontrados: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Manejo de errores generales
                _logger.LogError(ex, "Error al obtener las palabras frecuentes de los CVs");
                return StatusCode(500, $"Error al obtener palabras frecuentes: {ex.Message}");
            }
        }
    }
}
