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

        public void CreateUser(RegistrationDTO registration)
        {
            if (IsParamFormatOk(registration.UserName, registration.Password, registration.Email, registration.LastName, registration.FirstName))
            {
                if (!UserExist(registration.Email))
                {
                    var user = new User()
                    {
                        Id = Guid.NewGuid(),
                        UserName = registration.UserName,
                        Email = registration.Email,
                        LastName = registration.LastName,
                        FirstName = registration.FirstName,
                        EmailConfirmed = false,
                        NeedNewPassword = false,
                        CreatedOn = DateTime.UtcNow,
                        PhoneNumber = registration.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        PasswordHash = SecurePasswordHasher.Hash(registration.Password)
                    };
                    unitOfWork.userRepository.AddUser(user);
                    emailSender.ConfirmedEmail(user);

                        Address address = new Address()
                        {
                            Id = Guid.NewGuid(),
                            City = !string.IsNullOrWhiteSpace(registration.City) ? registration.City : "",
                            Street = !string.IsNullOrWhiteSpace(registration.Street) ? registration.Street : "",
                            HouseNumber = !string.IsNullOrWhiteSpace(registration.HouseNumber) ? registration.HouseNumber : "",
                            UserId = user.Id,
                            User = user
                        };
                    unitOfWork.addressRepository.AddAddress(address);
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
            if (string.IsNullOrWhiteSpace(username)) { throw new InvalidDataException("The username field is empty"); }
            if (string.IsNullOrWhiteSpace(password)) { throw new InvalidDataException("The password field is empty"); }
            if (string.IsNullOrWhiteSpace(email)) { throw new InvalidDataException("The email field is empty"); }
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
                password += passwordFrom[random.Next(passwordFrom.Length - 1)];
            }
            return password;
        }

        public UserDTO GetUser(string email/*, bool getAll*/)
        {
            //if (getAll == false)
            //{
            //var users = new List<UserDTO>();
            var user = unitOfWork.userRepository.GetByCriteria(x => x.Email == email).FirstOrDefault();
            var roles = unitOfWork.roleRepository.GetByUserId(user.Id);
            var address = unitOfWork.addressRepository.GetByUserId(user.Id);
            var rolesDTO = new List<RoleDTO>();
            foreach (var role in roles)
            {
                var roleDTO = new RoleDTO()
                {
                    Name = role.Name
                };
                rolesDTO.Add(roleDTO);
            }
            var claimsDTO = new List<ClaimDTO>();
            foreach (var claim in user.Claim)
            {
                var claimDTO = new ClaimDTO()
                {
                    ClaimType = claim.ClaimType,
                    ClaimValue = claim.ClaimValue
                };
                claimsDTO.Add(claimDTO);
            }
            var addressdto = new AddressDTO()
            {
                City = address.City,
                Street = address.Street,
                HouseNumber = address.HouseNumber
            };
            var userDTO = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Claim = claimsDTO,
                Role = rolesDTO,
                Address = addressdto
            };
            //users.Add(userDTO);
            //var UsersDTO = new UsersDTO()
            //{
            //    userDTOs = users
            //};
            return userDTO;
            //}
            //else
            //{
            //    var users = new List<UserDTO>();
            //    var user = unitOfWork.userRepository.GetByCriteria(x => x.Id != null).ToList();
            //    foreach (var item in user)
            //    {
            //        var roles = unitOfWork.roleRepository.GetByUserId(item.Id);
            //        var addressDTO = new List<AddressDTO>();
            //        foreach (var address in item.Address)
            //        {
            //            var addressDto = new AddressDTO()
            //            {
            //                City = address.City,
            //                Street = address.Street,
            //                HouseNumber = address.HouseNumber
            //            };
            //            addressDTO.Add(addressDto);
            //        }
            //        var rolesDTO = new List<RoleDTO>();
            //        foreach (var role in roles)
            //        {
            //            var roleDTO = new RoleDTO()
            //            {
            //                Name = role.Name
            //            };
            //            rolesDTO.Add(roleDTO);
            //        }
            //        var claimsDTO = new List<ClaimDTO>();
            //        foreach (var claim in item.Claim)
            //        {
            //            var claimDTO = new ClaimDTO()
            //            {
            //                ClaimType = claim.ClaimType,
            //                ClaimValue = claim.ClaimValue
            //            };
            //            claimsDTO.Add(claimDTO);
            //        }

            //        var userDTO = new UserDTO()
            //        {
            //            Id = item.Id,
            //            Email = item.Email,
            //            FirstName = item.FirstName,
            //            LastName = item.LastName,
            //            PhoneNumber = item.PhoneNumber,
            //            UserName = item.UserName,
            //            Claim = claimsDTO,
            //            Role = rolesDTO,
            //            Address = addressDTO
            //        };
            //        users.Add(userDTO);
            //    }
            //    var UsersDTO = new UsersDTO()
            //    {
            //        userDTOs = users
            //    };
            //    return UsersDTO;
            //}
        }

        public void ChangeDetails(string email, ChangesDTO change)
        {
            var user = unitOfWork.userRepository.GetUserByEmail(email);

            if (!string.IsNullOrWhiteSpace(change.NewPassword))
            {
                user.PasswordHash = SecurePasswordHasher.Hash(change.NewPassword);
                unitOfWork.userRepository.UpdateUser(user);
            }
            if (!string.IsNullOrWhiteSpace(change.PhoneNumber))
            {
                user.PhoneNumber = change.PhoneNumber;
                unitOfWork.userRepository.UpdateUser(user);
            }
            if (!string.IsNullOrWhiteSpace(change.City) || !string.IsNullOrWhiteSpace(change.Street) || !string.IsNullOrWhiteSpace(change.HouseNumber))
            {
                var oldAddress = unitOfWork.addressRepository.GetByUserId(user.Id);
                if (oldAddress == null)
                {
                    Address newAddress = new Address()
                    {
                        Id = Guid.NewGuid(),
                        City = string.IsNullOrWhiteSpace(change.City) ? change.City : "",
                        Street = string.IsNullOrWhiteSpace(change.Street) ? change.Street : "",
                        HouseNumber = string.IsNullOrWhiteSpace(change.HouseNumber) ? change.HouseNumber : "",
                        User = user,
                    };
                    unitOfWork.addressRepository.AddAddress(newAddress);
                }
                else
                {
                    if (!string.IsNullOrEmpty(change.City))
                    {
                        oldAddress.City = change.City;
                    }
                    if (!string.IsNullOrEmpty(change.Street))
                    {
                        oldAddress.Street = change.Street;
                    }
                    if (!string.IsNullOrEmpty(change.HouseNumber))
                    {
                        oldAddress.HouseNumber = change.HouseNumber;
                    }
                    unitOfWork.addressRepository.UpdateAddress(oldAddress);
                }
            }

        }

        public string IsUserExist(string email, string userName)
        {
            Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            var isEmailValid = validateEmailRegex.IsMatch(email);
            if (isEmailValid)
            {
                string exist = "";
                var isEmail = unitOfWork.userRepository.GetByCriteria(x => x.Email == email).FirstOrDefault();
                if (isEmail != null)
                {
                    exist += "Email ";
                }
                var isUserName = unitOfWork.userRepository.GetByCriteria(x => x.UserName == userName).FirstOrDefault();
                if (isUserName != null)
                {
                    exist += "UserName";
                }
                return exist;
            }
            return "NotValidEmail";
        }
    }
}

