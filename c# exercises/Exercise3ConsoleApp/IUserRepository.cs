public interface IUserRepository
{
    AuthenticationResult Authenticate(string userName, string password);
}
