using IdentityServer.Repositories;

namespace IdentityServer
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        IRoleRepository roleRepository { get; }
        IClaimRepository claimRepository { get; }
        IAddressRepository addressRepository { get; }



        void Complete();
        void Dispose();
    }
}