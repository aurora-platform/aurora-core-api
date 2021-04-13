namespace AuroraCore.Application.Interfaces
{
    public interface IHashProvider
    {
        bool IsEqual(string password, string hash);

        string HashPassword(string password);
    }
}
