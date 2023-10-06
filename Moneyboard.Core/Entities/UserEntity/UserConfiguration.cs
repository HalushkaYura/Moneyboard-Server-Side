using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Entities.UserEntity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.BirthDate)
                .IsRequired();

            builder
                .Property(x => x.Firstname)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(x => x.Lastname)
                .IsRequired()
                .HasMaxLength(50);
                


            builder
                .Property(x => x.CardNumber)
                .HasMaxLength(16)
                .IsRequired();

            builder
                .Property(x => x.CreateDate)
                .IsRequired();

            builder
                .HasMany(x => x.UserProjects)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);


        }
    }
}
