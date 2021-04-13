using AuroraCore.Application.Interfaces;
using AuroraCore.Application.Services;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.Infrastructure.Repositories;
using AuroraCore.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace aurora_core_api.Utils
{
    public static class DependencyInjectionExtensions
    {
		public static void AddDependencies(this IServiceCollection services)
		{
            var mapper = MapperFactory.Create();

            services.AddSingleton<IAuthenticationService>(new AuthenticationService(
                new UserRepository(),
                new BcryptHashProvider()
            ));
            services.AddSingleton<ITopicService>(new TopicService(
                new TopicRepository()
            ));


            services.AddSingleton<IUserService>(new UserService(
                new UserRepository(),
                mapper,
                new BcryptHashProvider()
            ));
        
            services.AddSingleton<IChannelService>(new ChannelService(
                new ChannelRepository(),
                new UserRepository(),
                new CloudinaryImageStorageService(),
                mapper
            ));

            services.AddSingleton<IJwtTokenProvider>(new JwtTokenProvider());
        }
	}
}
