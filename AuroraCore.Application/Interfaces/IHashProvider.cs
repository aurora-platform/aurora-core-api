namespace AuroraCore.Application.Interfaces
{
    public interface IHashProvider
    {
        void Verify(string password, string hash);

        string HashPassword(string password);
    }
}
