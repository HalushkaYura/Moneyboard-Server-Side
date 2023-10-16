using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Moneyboard.Core.Entities.UserProjectEntity
{
    public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.HasKey(x => x.UserProjectId);

            builder.Property(x => x.IsOwner)               
                .IsRequired();

            builder.Property(x => x.MemberDate)
                .IsRequired();

            builder.Property(x => x.PersonalPoints)
                 .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserProjects)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Project)
                .WithMany(x => x.UserProjects)
                .HasForeignKey(x => x.ProjectId);

            builder.HasOne(x => x.Role)
                .WithMany(xr => xr.UserProjects)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
