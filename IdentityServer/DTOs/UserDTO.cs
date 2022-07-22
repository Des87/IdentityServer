using IdentityServer.Models;

namespace IdentityServer.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<RoleDTO> Role { get; set; }
        public virtual List<Claim> Claim { get; set; }
    }
}
