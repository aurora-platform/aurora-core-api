using Amazon;
using Amazon.Runtime.CredentialManagement;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.Infrastructure.Services;
using AuroraCore.Web.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Infra = AuroraCore.Infrastructure.Startup;

namespace AuroraCore.Web
{
    public class Startup
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = "AKIA4QTFP2NPEYLZZY6C",
                SecretKey = "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"
            };

            var profile = new CredentialProfile("default", options)
            {
                Region = RegionEndpoint.USWest2
            };

            var sharedFile = new SharedCredentialsFile();
            sharedFile.RegisterProfile(profile);

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

            Infra.Configure();
        }
    }
}