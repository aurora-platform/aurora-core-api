using AuroraCore.Application.DTOs;
using AuroraCore.Application.DTOs.Channel;
using AuroraCore.Application.DTOs.Minidoc;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Utils;

namespace AuroraCore.Infrastructure.Factories
{
    public class MapperFactory
    {
        public static IObjectMapper Create()
        {
            var configuration = new AutoMapper.MapperConfiguration(config =>
            {
                config.CreateMap<User, UserResource>();
                config.CreateMap<User, UserCompact>();

                config.CreateMap<Channel, ChannelResource>();
                config.CreateMap<Channel, ChannelCompact>();

                config.CreateMap<Minidoc, MinidocResource>();
                config.CreateMap<Minidoc, MinidocCompact>();

                config.CreateMap<ImageReference, ImageCompact>();
            });

            return new ObjectMapper(configuration.CreateMapper());
        }
    }
}