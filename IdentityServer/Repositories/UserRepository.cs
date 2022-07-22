using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDb identityDb;

        public UserRepository(IdentityDb identityDb)
        {
            this.identityDb = identityDb;
        }
        public void AddUser(User user)
        {
            identityDb.User.Add(user);
            identityDb.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            identityDb.User.Update(user);
            identityDb.SaveChanges();
        }
        public User GetUserByEmail(string email)
        {
            return identityDb.User.First(x => x.Email == email);
        }
        public IQueryable<User> GetByCriteria(Expression<Func<User, bool>> predicate, bool includeRelatedEntities = true)
        {
            var user = identityDb.User.Where(predicate);
            if (includeRelatedEntities)
            {
                user = user.Include(r => r.Claim).Include(r => r.UserRole);
            }
            return user;
        }
        public bool CheckUserExist(string email)
        {
            return identityDb.User.Any(x => x.Email == email);
        }
    }
}
