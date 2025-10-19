using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create blacklisted_tokens table (the correct table name that already exists)
            // Note: If blacklisted_tokens table already exists, this migration may need to be skipped
            migrationBuilder.CreateTable(
                name: "blacklisted_tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blacklisted_tokens", x => x.Id);
                });

            // csd_user_registration table already exists in database, so we skip creating it
            // migrationBuilder.CreateTable(
            //     name: "csd_user_registration",
            //     columns: table => new
            //     {
            //         user_id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         email = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         password = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         account_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         role = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_csd_user_registration", x => x.user_id);
            //     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Only drop blacklisted_tokens table since we didn't create csd_user_registration
            migrationBuilder.DropTable(
                name: "blacklisted_tokens");

            // Don't drop csd_user_registration as it existed before this migration
            // migrationBuilder.DropTable(
            //     name: "csd_user_registration");
        }
    }
}
