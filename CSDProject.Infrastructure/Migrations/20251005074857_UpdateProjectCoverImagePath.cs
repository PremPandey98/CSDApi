using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectCoverImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCoverImage",
                table: "csd_Student_ProjectDetails");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCoverImagePath",
                table: "csd_Student_ProjectDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCoverImagePath",
                table: "csd_Student_ProjectDetails");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProjectCoverImage",
                table: "csd_Student_ProjectDetails",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
