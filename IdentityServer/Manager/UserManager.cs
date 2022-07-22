using IdentityServer.DTOs;
using IdentityServer.Helper;
using IdentityServer.Models;
using IdentityServer.Repositories;
using System.Text;
using System.Text.RegularExpressions;

namespace IdentityServer.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly EmailSender emailSender;

        public UserManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.emailSender = new EmailSender();
        }

        public void CreateUser(string username, string password, string email, string lastname, string fisrtname, string phonenumber)
        {
            if (IsParamFormatOk(username, password, email, lastname, fisrtname))
            {
                if (!UserExist(email))
                {
                    var user = new User()
                    {
                        Id = Guid.NewGuid(),
                        UserName = username,
                        Email = email,
                        LastName = lastname,
                        FirstName = fisrtname,
                        EmailConfirmed = false,
                        NeedNewPassword = false,
                        CreatedOn = DateTime.UtcNow,
                        PhoneNumber = phonenumber,
                        PhoneNumberConfirmed = false,
                        PasswordHash = SecurePasswordHasher.Hash(password)
                    };
                    emailSender.ConfirmedEmail(user);
                    unitOfWork.userRepository.AddUser(user);
                }
                else
                {
                    throw new InvalidDataException("The user with email has already been registered");
                }
            }
        }

        public void ConfirmEmail(string authToken)
        {
            var email = TokenHelper.GetUserFromToken(authToken);
            var user = unitOfWork.userRepository.GetUserByEmail(email);
            user.EmailConfirmed = true;
            unitOfWork.userRepository.UpdateUser(user);
        }

        private bool IsParamFormatOk(string username, string password, string email, string lastname, string fisrtname)
        {
            Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            if (string.IsNullOrWhiteSpace(username)) { throw new InvalidDataException("The username field is empty"); }
            if (string.IsNullOrWhiteSpace(password)) { throw new InvalidDataException("The password field is empty"); }
            if (string.IsNullOrWhiteSpace(email)) { throw new InvalidDataException("The email field is empty"); }
            if (!validateEmailRegex.IsMatch(email)) { throw new InvalidDataException("The email address is invalid format"); }
            if (string.IsNullOrWhiteSpace(lastname)) { throw new InvalidDataException("The lastname field is empty"); }
            if (string.IsNullOrWhiteSpace(fisrtname)) { throw new InvalidDataException("The firstname field is empty"); }
            return true;
        }

        public void GetConfirmEmail(string email)
        {
            var user = unitOfWork.userRepository.GetUserByEmail(email);
            if (user.EmailConfirmed == true) { throw new InvalidDataException("The email address is already confirmed"); }
            emailSender.ConfirmedEmail(user);
        }

        private bool UserExist(string email)
        {
            return unitOfWork.userRepository.CheckUserExist(email);
        }

        public void GetResetPasswordEmail(string email, string userName)
        {
            var user = unitOfWork.userRepository.GetUserByEmail(email);
            if (user.UserName == userName)
            {
                var newPassword = GeneratePassword(email, userName);
                user.NeedNewPassword = true;
                user.PasswordHash = SecurePasswordHasher.Hash(newPassword);
                unitOfWork.userRepository.UpdateUser(user);
                emailSender.SendNewPassword(user, newPassword);
            }
            else
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
        }

        private string GeneratePassword(string email, string userName)
        {
            var random = new Random();
            var charNumber = random.Next(10, 16);
            var passwordFrom = email + userName;
            var password = "";
            for (int i = 0; i < charNumber; i++)
            {
                password += passwordFrom[random.Next(passwordFrom.Length-1)];
            }
            return password;
        }

        public UserDTO GetUser(string email)
        {
            var user = unitOfWork.userRepository.GetByCriteria(x => x.Email == email).FirstOrDefault();
            var roles = unitOfWork.roleRepository.GetByUserId(user.Id);
            var rolesDTO = new List<RoleDTO>();
            foreach (var role in roles)
            {
                var roleDTO = new RoleDTO()
                {
                    Name = role.Name
                };
                rolesDTO.Add(roleDTO);
            }
            var userDTO = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Claim = user.Claim,
                Role = rolesDTO
            };
            return userDTO;
        }

        public void ChangePassword(string email, string newPassword)
        {
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                var user = unitOfWork.userRepository.GetUserByEmail(email);
                user.PasswordHash = SecurePasswordHasher.Hash(newPassword);
                unitOfWork.userRepository.UpdateUser(user);
            }
           
        }

        public void ChangePhonaNumber(string email, string newPhoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                var user = unitOfWork.userRepository.GetUserByEmail(email);
                user.PhoneNumber = newPhoneNumber;
                unitOfWork.userRepository.UpdateUser(user);
            }
        }
    }
}
