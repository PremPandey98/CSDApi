using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectApprovalWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalRequestedAt",
                table: "csd_Student_ProjectDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "csd_Student_ProjectDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalToken",
                table: "csd_Student_ProjectDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "csd_Student_ProjectDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                table: "csd_Student_ProjectDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpiresAt",
                table: "csd_Student_ProjectDetails",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalRequestedAt",
                table: "csd_Student_ProjectDetails");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "csd_Student_ProjectDetails");

            migrationBuilder.DropColumn(
                name: "ApprovalToken",
                table: "csd_Student_ProjectDetails");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "csd_Student_ProjectDetails");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                table: "csd_Student_ProjectDetails");

            migrationBuilder.DropColumn(
                name: "TokenExpiresAt",
                table: "csd_Student_ProjectDetails");
        }
    }
}
