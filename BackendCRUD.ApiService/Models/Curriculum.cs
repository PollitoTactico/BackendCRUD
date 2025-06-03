namespace BackendCRUD.ApiService.Models
{
    public class Curriculum
    {
        public int Id { get; set; }
        public string NombreCandidato { get; set; }
        public byte[] PDFCurriculum { get; set; }
        public DateTime FechaSubida { get; set; } = DateTime.Now;
        public string TextoExtraido { get; set; } // Para almacenar el texto del PDF
        public int? ProfileUserId { get; set; } // Relación con el perfil
        public ProfileUser ProfileUser { get; set; }
    }
}   
    