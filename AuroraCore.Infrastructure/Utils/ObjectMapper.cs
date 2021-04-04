using AuroraCore.Application.Dependencies;

namespace AuroraCore.Infrastructure.Utils
{
    public class ObjectMapper : IObjectMapper
    {
        private readonly AutoMapper.IMapper _mapper;

        public ObjectMapper(AutoMapper.IMapper mapper)
        {
            _mapper = mapper;
        }

        public T Map<T>(object origin)
        {
            return _mapper.Map<T>(origin);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
