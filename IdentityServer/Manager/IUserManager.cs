using IdentityServer.DTOs;

namespace IdentityServer.Manager
{
    public interface IUserManager
    {
        void ConfirmEmail(string authToken);
        void CreateUser(string username, string password, string email, string lastname, string fisrtname, string phonenumber);
        void GetConfirmEmail(string email);
        void GetResetPasswordEmail(string email, string userName);
        UserDTO GetUser(string email);
        void ChangePassword(string email, string newPassword);
        void ChangePhonaNumber(string email, string newPhoneNumber);
    }
}