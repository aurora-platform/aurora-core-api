namespace AuroraCore.Application.Interfaces
{
    public interface IObjectMapper
    {
        TDestination Map<TDestination>(object origin);
    }
}