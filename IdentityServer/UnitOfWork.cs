using IdentityServer.Helper;
using IdentityServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly dbContext dbContext;
        public IUserRepository userRepository { get; private set; }
        public IRoleRepository roleRepository { get; private set; }
        public IClaimRepository claimRepository { get; private set; }
        public IAddressRepository addressRepository { get; private set; }


        public UnitOfWork()
        {
            this.dbContext = new dbContext();
            this.userRepository = new UserRepository(dbContext);
            this.roleRepository = new RoleRepository(dbContext);
            this.claimRepository = new ClaimRepository(dbContext);
            this.addressRepository = new AddressRepository(dbContext);


        }
        public UnitOfWork(dbContext dbContext)
        {
            this.dbContext = dbContext;
            this.userRepository = new UserRepository(dbContext);
            this.roleRepository = new RoleRepository(dbContext);
            this.claimRepository = new ClaimRepository(dbContext);
            this.addressRepository = new AddressRepository(dbContext);

        }
        public void Complete()
        {
            dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
