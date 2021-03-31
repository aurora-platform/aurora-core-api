namespace AuroraCore.Application.Dependencies
{
    public interface IHashProvider
    {
        void Verify(string password, string hash);

        string HashPassword(string password);
    }
}
