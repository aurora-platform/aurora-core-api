namespace AuroraCore.Application.Dependencies
{
    public interface IObjectMapper
    {
        TDestination Map<TDestination>(object origin);

        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
