using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNoticeAndAnnouncementTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "csd_announcements",
                columns: table => new
                {
                    announcement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    target_audience = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_pinned = table.Column<bool>(type: "bit", nullable: false),
                    attachment_path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    view_count = table.Column<int>(type: "int", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csd_announcements", x => x.announcement_id);
                    table.ForeignKey(
                        name: "FK_csd_announcements_csd_user_registration_created_by",
                        column: x => x.created_by,
                        principalTable: "csd_user_registration",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "csd_notices",
                columns: table => new
                {
                    notice_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    target_audience = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_pinned = table.Column<bool>(type: "bit", nullable: false),
                    attachment_path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    view_count = table.Column<int>(type: "int", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csd_notices", x => x.notice_id);
                    table.ForeignKey(
                        name: "FK_csd_notices_csd_user_registration_created_by",
                        column: x => x.created_by,
                        principalTable: "csd_user_registration",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_csd_announcements_created_by",
                table: "csd_announcements",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_csd_notices_created_by",
                table: "csd_notices",
                column: "created_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csd_announcements");

            migrationBuilder.DropTable(
                name: "csd_notices");
        }
    }
}
