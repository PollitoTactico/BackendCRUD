using BackendCRUD.ApiService.Data;
using BackendCRUD.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCRUD.ApiService.Repository
{
    public class ProfileUserRepository
    {

        private readonly ApplicationDbContext _context;

        public ProfileUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileUser> GetProfileByIdAsync(int profileId)
        {
            return await _context.ProfileUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.id == profileId);
        }
    }
}
