using AuroraCore.Application.Interfaces;
using AuroraCore.Application.Services;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.Infrastructure.Repositories;
using AuroraCore.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuroraCore.Web.Utils
{
    public static class DependencyInjectionExtensions
    {
		public static void AddDependencies(this IServiceCollection services)
		{
            var mapper = MapperFactory.Create();
            var topicRepository = new TopicRepository();
            var channelRepository = new ChannelRepository();
            var userRepository = new UserRepository();
            var bcryptHashProvider = new BcryptHashProvider();

            services.AddSingleton<IAuthenticationService>(new AuthenticationService(userRepository, bcryptHashProvider, mapper));
            services.AddSingleton<ITopicService>(new TopicService(topicRepository));
            services.AddSingleton<IUserService>(new UserService(userRepository, mapper, bcryptHashProvider));
            services.AddSingleton<IChannelService>(new ChannelService(channelRepository, userRepository, new CloudinaryImageStorageService(), mapper));
            services.AddSingleton<IMinidocService>(new MinidocService(channelRepository, topicRepository, new MinidocRepository(), userRepository, mapper, new GoogleCloudStorage()));
            services.AddSingleton<IJwtTokenProvider>(new JwtTokenProvider());
        }
	}
}
