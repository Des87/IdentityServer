using IdentityServer.Models;

namespace IdentityServer.Repositories
{
    public interface IAddressRepository
    {
        void AddAddress(Address address);
        Address GetByUserId(Guid userId);
        void UpdateAddress(Address address);
    }
}