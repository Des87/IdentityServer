using IdentityServer.DTOs;

namespace IdentityServer.Manager
{
    public interface IUserManager
    {
        void ChangeDetails(string email, ChangesDTO changes);
        void ConfirmEmail(string authToken);
        void CreateUser(RegistrationDTO registration);
        void GetConfirmEmail(string email);
        void GetResetPasswordEmail(string email, string userName);
        UserDTO GetUser(string email);
        string IsUserExist(string email, string userName);
    }
}