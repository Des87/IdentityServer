using IdentityServer.Models;
using System.Linq.Expressions;

namespace IdentityServer.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        bool CheckUserExist(string email);
        User GetUserByEmail(string email);
        void UpdateUser(User user);
        IQueryable<User> GetByCriteria(Expression<Func<User, bool>> predicate, bool includeRelatedEntities = true);
    }
}