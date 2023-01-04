using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        [MaxLength(100)]
        public string? Street { get; set; }
        [MaxLength(100)]
        public string? HouseNumber { get; set; }
        public virtual User User { get; set; }
        public Guid? UserId { get; set; }
    }
}