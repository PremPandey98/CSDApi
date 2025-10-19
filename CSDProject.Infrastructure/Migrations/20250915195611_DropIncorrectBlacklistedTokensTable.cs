using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropIncorrectBlacklistedTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the incorrectly named BlacklistedTokens table
            // We'll use the existing blacklisted_tokens table instead
            migrationBuilder.DropTable(
                name: "BlacklistedTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate BlacklistedTokens table if we need to rollback
            migrationBuilder.CreateTable(
                name: "BlacklistedTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedTokens", x => x.Id);
                });
        }
    }
}
