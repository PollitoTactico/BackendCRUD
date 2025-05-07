using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCRUD.ApiService.Data;
using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.DTos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendCRUD.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfileUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProfileUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileUserDTO>>> GetProfileUsers()
        {
            var profiles = await _context.ProfileUsers.ToListAsync();

            var profileDTOs = profiles.Select(p => new ProfileUserDTO
            {
                Id = p.id,
                NombrePerfil = p.NombrePerfil,
                MisionCargo = p.MisionCargo,
                Empresa = p.Empresa,
                TituloCargo = p.TituloCargo,
                Departamento = p.Departamento,
                FormacionAcademica = p.FormacionAcademica,
                ConocimientosCargo = p.ConocimientosCargo,
                Experiencia = p.Experiencia,
                FuncionesEsenciales = p.FuncionesEsenciales,
                ConocimientoTecnologico = p.ConocimientoTecnologico
            }).ToList();

            return profileDTOs;
        }

        // GET: api/ProfileUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileUserDTO>> GetProfileUser(int id)
        {
            var profileUser = await _context.ProfileUsers.FindAsync(id);

            if (profileUser == null)
            {
                return NotFound();
            }

            var profileDTO = new ProfileUserDTO
            {
                Id = profileUser.id,
                NombrePerfil = profileUser.NombrePerfil,
                MisionCargo = profileUser.MisionCargo,
                Empresa = profileUser.Empresa,
                TituloCargo = profileUser.TituloCargo,
                Departamento = profileUser.Departamento,
                FormacionAcademica = profileUser.FormacionAcademica,
                ConocimientosCargo = profileUser.ConocimientosCargo,
                Experiencia = profileUser.Experiencia,
                FuncionesEsenciales = profileUser.FuncionesEsenciales,
                ConocimientoTecnologico = profileUser.ConocimientoTecnologico
            };

            return profileDTO;
        }

        // POST: api/ProfileUser
        [HttpPost]
        public async Task<ActionResult<ProfileUserDTO>> CreateProfileUser(ProfileUserCreateDTO profileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileUser = new ProfileUser
            {
                NombrePerfil = profileDTO.NombrePerfil,
                MisionCargo = profileDTO.MisionCargo,
                Empresa = profileDTO.Empresa,
                TituloCargo = profileDTO.TituloCargo,
                Departamento = profileDTO.Departamento,
                FormacionAcademica = profileDTO.FormacionAcademica,
                ConocimientosCargo = profileDTO.ConocimientosCargo,
                Experiencia = profileDTO.Experiencia,
                FuncionesEsenciales = profileDTO.FuncionesEsenciales,
                ConocimientoTecnologico = profileDTO.ConocimientoTecnologico
            };

            _context.ProfileUsers.Add(profileUser);
            await _context.SaveChangesAsync();

            var createdProfileDTO = new ProfileUserDTO
            {
                Id = profileUser.id,
                NombrePerfil = profileUser.NombrePerfil,
                MisionCargo = profileUser.MisionCargo,
                Empresa = profileUser.Empresa,
                TituloCargo = profileUser.TituloCargo,
                Departamento = profileUser.Departamento,
                FormacionAcademica = profileUser.FormacionAcademica,
                ConocimientosCargo = profileUser.ConocimientosCargo,
                Experiencia = profileUser.Experiencia,
                FuncionesEsenciales = profileUser.FuncionesEsenciales,
                ConocimientoTecnologico = profileUser.ConocimientoTecnologico
            };

            return CreatedAtAction(nameof(GetProfileUser), new { id = profileUser.id }, createdProfileDTO);
        }

        // PUT: api/ProfileUser/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfileUser(int id, ProfileUserUpdateDTO profileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileUser = await _context.ProfileUsers.FindAsync(id);
            if (profileUser == null)
            {
                return NotFound();
            }

            profileUser.NombrePerfil = profileDTO.NombrePerfil;
            profileUser.MisionCargo = profileDTO.MisionCargo;
            profileUser.Empresa = profileDTO.Empresa;
            profileUser.TituloCargo = profileDTO.TituloCargo;
            profileUser.Departamento = profileDTO.Departamento;
            profileUser.FormacionAcademica = profileDTO.FormacionAcademica;
            profileUser.ConocimientosCargo = profileDTO.ConocimientosCargo;
            profileUser.Experiencia = profileDTO.Experiencia;
            profileUser.FuncionesEsenciales = profileDTO.FuncionesEsenciales;
            profileUser.ConocimientoTecnologico = profileDTO.ConocimientoTecnologico;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ProfileUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfileUser(int id)
        {
            var profileUser = await _context.ProfileUsers.FindAsync(id);
            if (profileUser == null)
            {
                return NotFound();
            }

            _context.ProfileUsers.Remove(profileUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileUserExists(int id)
        {
            return _context.ProfileUsers.Any(e => e.id == id);
        }
    }
}