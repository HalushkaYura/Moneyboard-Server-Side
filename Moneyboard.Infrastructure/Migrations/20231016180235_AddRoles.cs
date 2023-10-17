using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moneyboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                    migrationBuilder.CreateTable(
            name: "CustomRoles", // Змініть ім'я таблиці на свій розсуд
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CustomRoles", x => x.Id);
            });

            // Генерування GUID для ролей "Owner" і "Member"
            var ownerRoleGuid = Guid.NewGuid().ToString();
            var memberRoleGuid = Guid.NewGuid().ToString();

            migrationBuilder.InsertData(
                table: "CustomRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
            { ownerRoleGuid, "Owner", "OWNER", Guid.NewGuid().ToString() },
            { memberRoleGuid, "Member", "MEMBER", Guid.NewGuid().ToString() },
                });
        }

    }
}
