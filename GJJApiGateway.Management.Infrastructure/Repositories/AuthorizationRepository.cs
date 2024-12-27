using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly ManagementDbContext _context;

        public AuthorizationRepository(ManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Authorization>> GetAllAsync()
        {
            return await _context.Authorizations.ToListAsync();
        }

        public async Task<Authorization> GetByIdAsync(int id)
        {
            return await _context.Authorizations.FindAsync(id);
        }

        public async Task AddAsync(Authorization authorization)
        {
            await _context.Authorizations.AddAsync(authorization);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Authorization authorization)
        {
            _context.Authorizations.Update(authorization);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var authorization = await _context.Authorizations.FindAsync(id);
            if (authorization != null)
            {
                _context.Authorizations.Remove(authorization);
                await _context.SaveChangesAsync();
            }
        }
    }
}
