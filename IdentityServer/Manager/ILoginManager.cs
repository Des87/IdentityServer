namespace IdentityServer.Manager
{
    public interface ILoginManager
    {
        string GetToken(string email, string password);
    }
}