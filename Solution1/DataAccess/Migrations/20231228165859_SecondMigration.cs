using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightIdFK",
                table: "Tickets",
                column: "FlightIdFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Flights_FlightIdFK",
                table: "Tickets",
                column: "FlightIdFK",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Flights_FlightIdFK",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightIdFK",
                table: "Tickets");
        }
    }
}
