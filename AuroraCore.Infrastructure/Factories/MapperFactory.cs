using AuroraCore.Application.Dependencies;
using AuroraCore.Application.DTOs;
using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Utils;

namespace AuroraCore.Infrastructure.Factories
{
    public class MapperFactory
    {
        public static IObjectMapper Create()
        {
            var configuration = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfile, User>();
            });

            return new ObjectMapper(configuration.CreateMapper());
        }
    }
}
