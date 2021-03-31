namespace AuroraCore.Domain.Shared
{
    public class Validation
    {
        public static void NotNullOrWhiteSpace(string name, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(errorMessage);
            }
        }
    }
}
