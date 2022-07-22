using IdentityServer.Helper;
using IdentityServer.Repositories;

namespace IdentityServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDb identityDb;
        public IUserRepository userRepository { get; private set; }
        public IRoleRepository roleRepository { get; private set; }

        public UnitOfWork()
        {
            this.identityDb = new IdentityDb();
            this.userRepository = new UserRepository(identityDb);
            this.roleRepository = new RoleRepository(identityDb);

        }
        public void Complete()
        {
            identityDb.SaveChangesAsync();
        }
        public void Dispose()
        {
            identityDb.Dispose();
        }
    }
}
