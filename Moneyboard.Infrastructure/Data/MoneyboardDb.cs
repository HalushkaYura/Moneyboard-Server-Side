using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.RefreshTokenEntity;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using static Moneyboard.Core.Entities.RefreshTokenEntity.RefreshTocenConfiguration;
using Moneyboard.Infrastructure.Data.SeedData;
using Moneyboard.Core.Interfaces.Repository;

namespace Moneyboard.Infrastructure.Data
{
    public class MoneyboardDb : IdentityDbContext<User>
    {

        public MoneyboardDb(DbContextOptions<MoneyboardDb> options) : base(options)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new BankCardConfiguration());
            modelBuilder.ApplyConfiguration(new UserProjectConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            //modelBuilder.SeedRoles(_roleRepository);

        }

        public DbSet<Project> Project { get; set; }
        public DbSet<BankCard> BankCard { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserProject> UserProject { get; set; }

    }
}
