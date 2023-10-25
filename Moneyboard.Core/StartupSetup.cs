using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moneyboard.Core.Helpers;
using Moneyboard.Core.Helpers.Mails;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Services;
using Moneyboard.Core.Validation;

namespace Moneyboard.Core
{
    public static class StartupSetup
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IConfirmEmailService, ConfirmEmailService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ILocaleStorageService, LocaleStorageService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBankCardService, BanckCardService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSignalR();



        }

        /*public static void AddFileService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ =>
                new BlobServiceClient(configuration.GetSection("AzureBlobStorageSettings")
                    .GetValue<string>("AccessKey")));

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
            services.AddScoped<ILocaleStorageService, LocaleStorageService>();
        }*/

        public static void AddFluentValitation(this IServiceCollection services)
        {
            services.AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<UserLogValidation>());
        }

        public static void ConfigJwtOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtOption"));
        }

        public static void ConfigureMailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("EmailSettings"));
        }

        public static void ConfigureClientApplicationUrl(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ClientUrl>(configuration.GetSection("ClientServer"));
        }

        /*public static void ConfigureValidationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var configItem =
            configuration.GetSection("RolesAccess")
                .Get<Dictionary<WorkSpaceRoles, List<WorkSpaceRoles>>>();
            services.AddSingleton<RoleAccess>(new RoleAccess() { RolesAccess = configItem });
        }*/

        public static void ConfigureImageSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ImageSettings>(configuration.GetSection("ImageSettings"));
        }
        public static void ConfigureTaskAttachmentSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<AttachmentSettings>(configuration.GetSection("AttachmentSettings"));
        }

        public static void ConfigureFileSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileSettings>(configuration.GetSection("FileSettings"));
        }

        public static void AddAutoMapper(this IServiceCollection services)
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
            //services.Configure<RoleAccess>(configuration.GetSection("RolesAccess"));
        }
    }
}
