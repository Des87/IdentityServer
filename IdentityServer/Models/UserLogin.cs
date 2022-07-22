using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class UserLogin : IHasId
    {
        [Key]
        public Guid Id { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public virtual User User { get; set; }
    }
}
