using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityServer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly dbContext identityDb;

        public RoleRepository(dbContext identityDb)
        {
            this.identityDb = identityDb;
        }
        public List<Role> GetByUserId(Guid userId)
        {
            var userRoles = identityDb.UserRole.Where(x => x.UserId == userId);

            var roles = new List<Role>();
            foreach (var userRole in userRoles)
            {
                var role = identityDb.Role.FirstOrDefault(x => x.Id == userRole.RoleId);
                roles.Add(role);
            }
            return roles;
        }
    }
}
