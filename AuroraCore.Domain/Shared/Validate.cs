namespace AuroraCore.Domain.Shared
{
    public class Validate
    {
        public static void NotNullOrWhiteSpace(string name, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ValidationException(errorMessage);
        }

        public static void NotNull<T>(T obj, string errorMessage)
        {
            if (obj is null) throw new ValidationException(errorMessage);
        }
    }
}