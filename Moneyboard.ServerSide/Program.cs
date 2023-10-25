using Hangfire;
using Microsoft.AspNet.SignalR;
using Microsoft.EntityFrameworkCore;
using Moneyboard.Core;
using Moneyboard.Core.Helpers;
using Moneyboard.Core.SignalRChat;
using Moneyboard.Infrastructure;
using Moneyboard.WebApi.Middleweres;
using Moneyboard.WebApi.ServiceExtension;
using Newtonsoft.Json.Serialization;

namespace Moneyboard.ServerSide
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //ADDED
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            //

            services.AddControllers();
            services.AddDbContext(configuration.GetConnectionString("DefaultConnection"));
            services.AddIdentityDbContext();
            services.AddAuthentication();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            services.AddRepositories();
            services.AddCustomServices();
            services.AddFluentValitation();
            services.AddSwagger();
            services.ConfigureImageSettings(configuration);
            services.AddAutoMapper();
            services.AddJwtAuthentication(configuration);
            services.AddMvcCore().AddRazorViewEngine();

            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
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

            //ADDED
            builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            //

            var app = builder.Build();
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //ADDED
            app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            //
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //
            //app.MapHub<SignalRHub>("/notificationHub");

            //
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}