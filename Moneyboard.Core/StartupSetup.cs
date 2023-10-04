using Castle.Core.Configuration;
using Maneyboard.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Moneyboard.Core.Helpers;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Roles;
using Moneyboard.Core.Services;
using System.IO;

namespace Moneyboard.Core
{
    public static class StartupSetup
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtService, JwtService>();
        }


        public static void AddFluentValitation(this IServiceCollection services)
        {
            //  services.AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<UserLogValidation>());
        }

        public static void ConfigJwtOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOptions>((Microsoft.Extensions.Configuration.IConfiguration)config);
        }
        public static void AddAuthentification(this IServiceCollection services)
        {
            services.AddScoped<IAuthentificationServices, AuthentificationServices>();
        }

        /* public static void ConfigureImageSettings(this IServiceCollection services, IConfiguration configuration)
         {
             services.Configure<ImageSettings>(configuration.GetSection("ImageSettings"));
         }*/


        /*public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigureRolesAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RoleAccess>(configuration.GetSection("RolesAccess"));
        }*/
    }
}