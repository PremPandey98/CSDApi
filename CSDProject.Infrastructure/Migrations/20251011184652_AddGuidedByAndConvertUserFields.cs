using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGuidedByAndConvertUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PublishedBy",
                table: "csd_Student_ProjectDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectCreatedBy",
                table: "csd_Student_ProjectDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuidedBy",
                table: "csd_Student_ProjectDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuidedBy",
                table: "csd_Student_ProjectDetails");

            migrationBuilder.AlterColumn<string>(
                name: "PublishedBy",
                table: "csd_Student_ProjectDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectCreatedBy",
                table: "csd_Student_ProjectDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
