using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomTourism.DataAccess.Migrations
{
    public partial class addCarsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeOfTransmission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOfSeats = table.Column<int>(type: "int", nullable: false),
                    TypeOfFuel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityDeposit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcessClaim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MilageCharge = table.Column<int>(type: "int", nullable: false),
                    MinimumEligibleAge = table.Column<int>(type: "int", nullable: false),
                    InsuranceIncluded = table.Column<bool>(type: "bit", nullable: false),
                    DailyCharge = table.Column<int>(type: "int", nullable: false),
                    WeeklyCharge = table.Column<int>(type: "int", nullable: false),
                    MonthlyCharge = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarImages_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarImages_CarId",
                table: "CarImages",
                column: "CarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarImages");

            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
