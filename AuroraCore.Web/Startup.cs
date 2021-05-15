using aurora_core_api.Utils;
using AuroraCore.Infrastructure.Logging;
using AuroraCore.Infrastructure.Providers;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new JsonPrivateResolver());

            services.AddDependencies();

            services.AddSwaggerGen(config => config.SwaggerDoc("v1", new OpenApiInfo { Title = "Aurora Core API v1", Version = "v1" }));

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "aurora_core_api"));
            }

            app.UseGlobalExceptionHandler(new SentryLogger());

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
