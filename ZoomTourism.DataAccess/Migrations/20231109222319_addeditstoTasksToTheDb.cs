using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomTourism.DataAccess.Migrations
{
    public partial class addeditstoTasksToTheDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskPriority",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskPriority",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "Tasks");
        }
    }
}
