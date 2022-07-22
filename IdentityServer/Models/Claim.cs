using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class Claim : IHasId
    {
        [Key]
        public Guid Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public virtual User User { get; set; }
    }
}
