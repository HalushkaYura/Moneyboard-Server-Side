using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moneyboard.Infrastructure.Data.Repositories;
using Google;
using Moneyboard.Core.Interfaces.Repository;

namespace Moneyboard.Infrastructure
{
    public static class StartupSetup
    {

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(Moneyboard.Core.Interfaces.Repository.IRepository<>), typeof(BaseRepository<>));

        }
    
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MoneyboardDb>(options => options.UseSqlServer(connectionString));
        }

        public static void AddIdentityDbContext(this IServiceCollection services)
        {
            services.AddIdentity<User,
                IdentityRole>().AddEntityFrameworkStores<MoneyboardDb>().AddDefaultTokenProviders();
        }
    }
}