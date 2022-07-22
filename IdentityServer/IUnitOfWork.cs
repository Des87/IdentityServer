using IdentityServer.Repositories;

namespace IdentityServer
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        IRoleRepository roleRepository { get; }


        void Complete();
        void Dispose();
    }
}