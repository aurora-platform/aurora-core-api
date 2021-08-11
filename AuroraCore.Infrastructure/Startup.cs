namespace AuroraCore.Infrastructure
{
    public class Startup
    {
        public static void Configure()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}