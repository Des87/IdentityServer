using IdentityServer.Helper;

namespace IdentityServer.Manager
{
    public class LoginManager : ILoginManager
    {
        private readonly IUnitOfWork unitOfWork;

        public LoginManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public string GetToken(string email, string password)
        {
            if (IsParamFormatOk(email, password))
            {
                var user = unitOfWork.userRepository.GetUserByEmail(email);
                if (SecurePasswordHasher.Verify(password, user.PasswordHash))
                {
                    return TokenHelper.GenerateJSONWebToken(user);
                }
                throw new UnauthorizedAccessException("Unauthorized");
            }
            throw new UnauthorizedAccessException("Unauthorized");
        }

        private bool IsParamFormatOk(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) { throw new InvalidDataException("The password field is empty"); }
            if (string.IsNullOrWhiteSpace(email)) { throw new InvalidDataException("The email field is empty"); }
            return true;
        }
    }
}
