using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomTourism.DataAccess.Migrations
{
    public partial class addUsersToLead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingDepId",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingDepUserId",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CallCenterId",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CallCenterUserId",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverUserId",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_BookingDepId",
                table: "Leads",
                column: "BookingDepId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CallCenterId",
                table: "Leads",
                column: "CallCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_DriverId",
                table: "Leads",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_AspNetUsers_BookingDepId",
                table: "Leads",
                column: "BookingDepId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_AspNetUsers_CallCenterId",
                table: "Leads",
                column: "CallCenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_AspNetUsers_DriverId",
                table: "Leads",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_AspNetUsers_BookingDepId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_AspNetUsers_CallCenterId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_AspNetUsers_DriverId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_BookingDepId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_CallCenterId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_DriverId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "BookingDepId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "BookingDepUserId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CallCenterId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CallCenterUserId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "DriverUserId",
                table: "Leads");
        }
    }
}
