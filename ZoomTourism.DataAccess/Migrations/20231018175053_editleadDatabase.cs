using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomTourism.DataAccess.Migrations
{
    public partial class editleadDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeadId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_LeadId",
                table: "Reviews",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Leads_LeadId",
                table: "Reviews",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Leads_LeadId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_LeadId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Reviews");
        }
    }
}
