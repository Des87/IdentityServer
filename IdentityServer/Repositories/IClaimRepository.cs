

using IdentityServer.Models;

namespace IdentityServer.Repositories
{
    public interface IClaimRepository
    {
        void AddClaim(Claim claim);
        List<Claim> GetByUserId(Guid userId);
        void UpdateClaim(Claim claim);
    }
}