using IdentityServer.Models;

namespace IdentityServer.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly dbContext dbContext;

        public AddressRepository(dbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddAddress(Address address)
        {
            dbContext.Address.Add(address);
            dbContext.SaveChanges();
        }
        public void UpdateAddress(Address address)
        {
            dbContext.Address.Update(address);
            dbContext.SaveChanges();
        }
        public Address GetByUserId(Guid userId)
        {
            var address = dbContext.Address.Where(x => x.User.Id == userId).FirstOrDefault();
            return address;
        }
    }
}