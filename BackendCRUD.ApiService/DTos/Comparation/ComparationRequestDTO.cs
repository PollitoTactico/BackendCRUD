using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendCRUD.ApiService.DTOs.Comparation
{
    public class ComparationRequestDTO
    {
        public int ProfileId { get; set; }
        public List<int> CvIds { get; set; }
    }
}
