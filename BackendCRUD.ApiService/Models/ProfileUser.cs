namespace BackendCRUD.ApiService.Models
{
    public class ProfileUser
    {
        public int id { get; set; }
        public string NombrePerfil { get; set; }  
        public string MisionCargo { get; set; }
        public string Empresa { get; set; } //combo box con el nombre de empresas relacionadas a SG 
        public string TituloCargo { get; set; } 
        public string Departamento { get; set; }
        public string FormacionAcademica { get; set; } //combo box con formaciones academicas, pregrados, postgrados, etc
        public string ConocimientosCargo { get; set; } 
        public string Experiencia { get; set; } 
        public string FuncionesEsenciales { get; set; }
        public string ConocimientoTecnologico { get; set; }
    }
}
