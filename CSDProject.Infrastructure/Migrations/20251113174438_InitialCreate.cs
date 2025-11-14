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

            migrationBuilder.CreateTable(
                name: "csd_Student_ContactUS",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MblNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csd_Student_ContactUS", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "csd_Student_ProjectDetails",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectCoverImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectCreatedBy = table.Column<int>(type: "int", nullable: true),
                    PublishedBy = table.Column<int>(type: "int", nullable: true),
                    GuidedBy = table.Column<int>(type: "int", nullable: true),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalRequestedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csd_Student_ProjectDetails", x => x.ProjectId);
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

            migrationBuilder.DropTable(
                name: "csd_Student_ContactUS");

            migrationBuilder.DropTable(
                name: "csd_Student_ProjectDetails");
        }
    }
}
