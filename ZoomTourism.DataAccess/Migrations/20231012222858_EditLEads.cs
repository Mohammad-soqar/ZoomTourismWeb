using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomTourism.DataAccess.Migrations
{
    public partial class EditLEads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false),
                    Destinations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedCarId = table.Column<int>(type: "int", nullable: true),
                    CarPickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CarReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HotelCheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HotelCheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HotelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelTotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leads_Cars_SelectedCarId",
                        column: x => x.SelectedCarId,
                        principalTable: "Cars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SelectedCarId",
                table: "Leads",
                column: "SelectedCarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leads");
        }
    }
}
