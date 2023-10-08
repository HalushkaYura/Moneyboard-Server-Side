using Microsoft.EntityFrameworkCore;
using Moneyboard.Core;
using Moneyboard.Core.Helpers;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Services;
using Moneyboard.Infrastructure;
using Moneyboard.WebApi.ServiceExtension;

namespace Moneyboard.ServerSide
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddDbContext(configuration.GetConnectionString("DefaultConnection"));
            services.AddIdentityDbContext();
            services.AddAuthentication();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            services.AddRepositories();
            services.AddCustomServices();
            services.AddFluentValitation();
            services.AddAuthentication();
            services.AddSwagger();
            services.ConfigureImageSettings(configuration);
            services.AddAutoMapper();
            services.AddJwtAuthentication(configuration);
            services.AddCors();
            services.AddMvcCore().AddRazorViewEngine();
        }
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);



            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}