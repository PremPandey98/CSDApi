using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsMobileDeviceLoginToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_mobile_device_login",
                table: "csd_user_registration",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_mobile_device_login",
                table: "csd_user_registration");
        }
    }
}
