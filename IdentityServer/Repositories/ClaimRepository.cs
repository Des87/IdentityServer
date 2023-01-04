using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly dbContext dbContext;

        public ClaimRepository(dbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddClaim(Claim claim)
        {
            dbContext.Claim.Add(claim);
            dbContext.SaveChanges();
        }
        public void UpdateClaim(Claim claim)
        {
            dbContext.Claim.Update(claim);
            dbContext.SaveChanges();
        }
        public List<Claim> GetByUserId(Guid userId)
        {
            var claims = dbContext.Claim.Where(x => x.User.Id == userId).ToList();
            return claims;
        }
    }
}
