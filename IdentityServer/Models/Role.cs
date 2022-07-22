using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class Role : IHasId
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<UserRole> UserRole { get; set; }
    }
}
