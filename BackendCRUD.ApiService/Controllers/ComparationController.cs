using BackendCRUD.ApiService.DTOs.Comparation;
using BackendCRUD.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BackendCRUD.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComparationController : ControllerBase
    {
        private readonly IComparationService _comparationService;

        public ComparationController(IComparationService comparationService)
        {
            _comparationService = comparationService;
        }

        [HttpPost("compare")]
        public async Task<IActionResult> Compare([FromBody] ComparationRequestDTO request)
        {
            try
            {
                var result = await _comparationService.CompareProfileWithCVs(request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al comparar: {ex.Message}");
            }
        }

        [HttpPost("frequent-words")]
        public async Task<IActionResult> GetFrequentWords([FromBody] List<int> cvIds)
        {
            try
            {
                var result = await _comparationService.GetMostFrequentWordsFromCVs(cvIds);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener palabras frecuentes: {ex.Message}");
            }
        }
    }
}
