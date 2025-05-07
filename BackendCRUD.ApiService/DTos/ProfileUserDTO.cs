using System.ComponentModel.DataAnnotations;

namespace BackendCRUD.ApiService.DTos
{
    public class ProfileUserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del perfil es obligatorio")]
        public string NombrePerfil { get; set; }

        [Required(ErrorMessage = "La misión del cargo es obligatoria")]
        public string MisionCargo { get; set; }

        [Required(ErrorMessage = "La empresa es obligatoria")]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "El título del cargo es obligatorio")]
        public string TituloCargo { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio")]
        public string Departamento { get; set; }

        [Required(ErrorMessage = "La formación académica es obligatoria")]
        public string FormacionAcademica { get; set; }

        public string ConocimientosCargo { get; set; }

        public string Experiencia { get; set; }

        public string FuncionesEsenciales { get; set; }

        public string ConocimientoTecnologico { get; set; }
    }

    public class ProfileUserCreateDTO
    {
        [Required(ErrorMessage = "El nombre del perfil es obligatorio")]
        public string NombrePerfil { get; set; }

        [Required(ErrorMessage = "La misión del cargo es obligatoria")]
        public string MisionCargo { get; set; }

        [Required(ErrorMessage = "La empresa es obligatoria")]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "El título del cargo es obligatorio")]
        public string TituloCargo { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio")]
        public string Departamento { get; set; }

        [Required(ErrorMessage = "La formación académica es obligatoria")]
        public string FormacionAcademica { get; set; }

        public string ConocimientosCargo { get; set; }

        public string Experiencia { get; set; }

        public string FuncionesEsenciales { get; set; }

        public string ConocimientoTecnologico { get; set; }
    }

    public class ProfileUserUpdateDTO
    {
        [Required(ErrorMessage = "El nombre del perfil es obligatorio")]
        public string NombrePerfil { get; set; }

        [Required(ErrorMessage = "La misión del cargo es obligatoria")]
        public string MisionCargo { get; set; }

        [Required(ErrorMessage = "La empresa es obligatoria")]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "El título del cargo es obligatorio")]
        public string TituloCargo { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio")]
        public string Departamento { get; set; }

        [Required(ErrorMessage = "La formación académica es obligatoria")]
        public string FormacionAcademica { get; set; }

        public string ConocimientosCargo { get; set; }

        public string Experiencia { get; set; }

        public string FuncionesEsenciales { get; set; }

        public string ConocimientoTecnologico { get; set; }
    }
}