using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Interfaces.Repository;
using Moneyboard.Core.Interfaces.Services;

namespace Moneyboard.Infrastructure.Data.SeedData
{

        public static class SeedData
        {
        public static void SeedRoles(this ModelBuilder modelBuilder, IRepository<Role> roleRepository)
        {
                var memberRole = new Role
                {
                    RoleName = "Member",
                    RolePoints = 0,
                    CreateDate = DateTime.Now
                };

                var ownerRole = new Role
                {
                    RoleName = "Owner",
                    RolePoints = 100, // або інша кількість балів за роль "Owner"
                    CreateDate = DateTime.Now
                };

                roleRepository.AddAsync(memberRole);
                roleRepository.AddAsync(ownerRole);
                roleRepository.SaveChangesAsync();
            }
        }
    
}
