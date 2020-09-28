using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Business;
using Service.Interfaces;
using Service.Utils.Helper.LoginConfiguration;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Athentication Login
            var signing = new Signing();
            services.AddSingleton(signing);

            var tokenConfigurations = new Token();

            new ConfigureFromConfigurationOptions<Token>(
                Configuration.GetSection("TokenConfigurations") //TokenConfigurations AppSettingsJson
            ).Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);//Configuring the three attributes in class getting the json settings

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signing.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Validates the signing of a received token
                paramsValidation.ValidateIssuerSigningKey = true;

                // Checks if a received token is still valid
                paramsValidation.ValidateLifetime = true;

                // Tolerance time for the expiration of a token (used in case
                // of time synchronization problems between different
                // computers involved in the communication process)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Enables the use of the token as a means of
            // authorizing access to this project's resources
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("user", policy => policy.RequireClaim("Store", "user"));
                auth.AddPolicy("admin", policy => policy.RequireClaim("Store", "admin"));
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser()
                    .Build());
                //adicionar filtros de autenticação
            });

            //Dependecy Injection
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped(typeof(IPlaylistRepository), typeof(PlaylistRepository));
            services.AddScoped(typeof(IPlaylistBusiness), typeof(PlaylistBusiness));
            services.AddScoped(typeof(IPlaylistService), typeof(PlaylistService));

            services.AddScoped(typeof(IWeatherRepository), typeof(WeatherRepository));
            services.AddScoped(typeof(IWeatherService), typeof(WeatherService));

            services.AddScoped(typeof(IUserSystemBusiness), typeof(UserSystemBusiness));

            services.AddCors();

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //Cache 
            services.AddMemoryCache();
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration =
            //        Configuration.GetConnectionString("ConnnectionRedis");
            //    options.InstanceName = "RedisCache";
            //});

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder => builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
