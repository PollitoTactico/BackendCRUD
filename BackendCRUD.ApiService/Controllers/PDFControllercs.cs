using Microsoft.AspNetCore.Mvc;
using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.Services.Interfaces;

namespace BackendCRUD.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PDFController : ControllerBase
    {
        private readonly ICVService _cvService;

        public PDFController(ICVService cvService)
        {
            _cvService = cvService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPDF([FromForm] IFormFile archivo, [FromForm] string nombreCandidato)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo no válido");

            if (!archivo.FileName.EndsWith(".pdf"))
                return BadRequest("Solo se permiten archivos PDF");

            if (archivo.Length > 5 * 1024 * 1024) // 5MB de límite
                return BadRequest("El archivo excede el tamaño máximo de 5MB");

            using var memoryStream = new MemoryStream();
            await archivo.CopyToAsync(memoryStream);

            var curriculum = new Curriculum
            {
                NombreCandidato = nombreCandidato,
                PDFCurriculum = memoryStream.ToArray(),
                FechaSubida = DateTime.Now
            };

            _cvService.AddCV(curriculum); // Usamos el servicio

            return Ok(new { id = curriculum.Id, message = "Archivo subido correctamente" });
        }

        [HttpGet("{id}")]
        public IActionResult GetPDF(int id)
        {
            var archivo = _cvService.GetCV(id); // Usamos el servicio
            if (archivo == null)
                return NotFound();

            return File(archivo.PDFCurriculum, "application/pdf", $"Curriculum_{archivo.NombreCandidato}.pdf");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePDF(int id)
        {
            var archivo = _cvService.GetCV(id);
            if (archivo == null)
                return NotFound();

            _cvService.DeleteCV(id); // Necesitarás agregar este método al servicio
            return Ok("Archivo eliminado correctamente");
        }
    }
}
