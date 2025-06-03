using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.Services.Interfaces;
namespace BackendCRUD.ApiService.Services.Implementations
{
    public class CVService : ICVService
    {
        private static readonly List<Curriculum> _cvs = new();
        private static int _nextId = 1;

        public void AddCV(Curriculum cv)
        {
            cv.Id = _nextId++;
            _cvs.Add(cv);
        }

        public Curriculum GetCV(int id) => _cvs.FirstOrDefault(c => c.Id == id);

        public List<Curriculum> GetCVs(List<int> ids) =>
            _cvs.Where(c => ids.Contains(c.Id)).ToList();

        public bool DeleteCV(int id)
        {
            var cv = GetCV(id);
            return cv != null && _cvs.Remove(cv);
        }
    }
}
