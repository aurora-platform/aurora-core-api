using AuroraCore.Application.Interfaces;
using AuroraCore.Application.Services;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace aurora_core_api.Utils
{
    public static class DependencyInjectionExtensions
    {
		public static void AddDependencies(this IServiceCollection services)
		{
            services.AddSingleton<IAuthenticationService>(new AuthenticationService(new UserRepository(), new BcryptHashProvider()));
            services.AddSingleton<ITopicService>(new TopicService(new TopicRepository()));
            services.AddSingleton<IUserService>(new UserService(new UserRepository(), MapperFactory.Create(), new BcryptHashProvider()));
            services.AddSingleton<IJwtTokenProvider>(new JwtTokenProvider());
        }
	}
}
