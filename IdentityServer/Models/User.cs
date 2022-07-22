using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class User : IHasId
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public bool NeedNewPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<UserLogin> UserLogin { get; set; }
        public virtual List<UserRole> UserRole { get; set; }
        public virtual List<Claim> Claim { get; set; }
    }
}
