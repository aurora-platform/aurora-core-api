using aurora_core_api.Utils;
using AuroraCore.Application.Dependencies;
using AuroraCore.Application.Interfaces;
using AuroraCore.Application.Services;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Infrastructure = AuroraCore.Infrastructure.Startup;

namespace aurora_core_api
{
    public class Startup
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _jwtTokenProvider = new JwtTokenProvider();
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<IAuthenticationService>(new AuthenticationService(new UserRepository(), new BcryptHashProvider()));
            services.AddSingleton<ITopicService>(new TopicService(new TopicRepository()));
            services.AddSingleton<IUserService>(new UserService(new UserRepository(), MapperFactory.Create(), new BcryptHashProvider()));
            services.AddSingleton<IJwtTokenProvider>(new JwtTokenProvider());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new JsonPrivateResolver();
            });

            ConfigureDependencies(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "aurora_core_api", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        SignatureValidator = (string token, TokenValidationParameters parameters) =>
                        {
                            if (_jwtTokenProvider.IsValid(token)) return new JwtSecurityToken(token);
                            return null;
                        },
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aurora Core API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Infrastructure.Configure();
        }
    }
}
