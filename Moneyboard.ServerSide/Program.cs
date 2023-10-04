using Microsoft.EntityFrameworkCore;
using Moneyboard.Core;
using Moneyboard.Core.Interfaces.Repository;
using Moneyboard.Infrastructure;
using Moneyboard.Infrastructure.Data.Repositories;

namespace Moneyboard.ServerSide
{
    public class Program
    {
        private readonly IConfiguration Configuration;

        public Program(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            //services.ConfigureImageSettings(Configuration);
            services.AddControllers();
            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.AddIdentityDbContext();
            services.AddAuthentication();
            services.AddRepositories();
            services.AddCustomServices();
            services.AddFluentValitation();
            //services.ConfigJwtOptions(_configuration.GetSection("JwtOptions"));
            services.AddAuthentification();
            //services.AddAutoMapper();

            //services.AddJwtAuthentication(Configuration);
            services.AddCors();
            services.AddMvcCore().AddRazorViewEngine();
        }
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            var program = new Program(builder.Configuration);
            program.ConfigureServices(builder.Services);



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