using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Moneyboard.Core.Entities.ProjectEntity
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .HasKey(x => x.ProjectId);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.Currency)
                .IsRequired();

            builder
                .Property(x => x.BaseSalary)
                .IsRequired();

            builder
                .Property(x => x.SalaryDate)
                .IsRequired()
                .HasColumnType("date");

            builder
                .Property(x => x.ProjectPoinPercent)
                .HasMaxLength(3)
                .IsRequired();

            builder
               .HasMany(x => x.UserProjects)
               .WithOne(x => x.Project)
               .HasForeignKey(x => x.ProjectId);
            builder
               .HasMany(x => x.Roles)
               .WithOne(x => x.Project)
               .HasForeignKey(x => x.ProjectId);

        }
    }
}
