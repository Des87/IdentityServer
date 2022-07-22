using IdentityServer.Models;
using System.Linq.Expressions;

namespace IdentityServer.Repositories
{
    public interface IRoleRepository
    {
        List<Role> GetByUserId(Guid userId);
    }
}