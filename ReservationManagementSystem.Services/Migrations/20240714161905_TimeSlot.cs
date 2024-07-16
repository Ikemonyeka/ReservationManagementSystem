using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManagementSystem.Services.Migrations
{
    /// <inheritdoc />
    public partial class TimeSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationsAvailability");

            migrationBuilder.CreateTable(
                name: "ReservationTimeSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeSlot = table.Column<TimeOnly>(type: "time", nullable: false),
                    Duration = table.Column<TimeOnly>(type: "time", nullable: false),
                    RestuarantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationTimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationTimeSlots_Restuarants_RestuarantId",
                        column: x => x.RestuarantId,
                        principalTable: "Restuarants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTimeSlots_RestuarantId",
                table: "ReservationTimeSlots",
                column: "RestuarantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationTimeSlots");

            migrationBuilder.CreateTable(
                name: "ReservationsAvailability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestuarantId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationsAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationsAvailability_Restuarants_RestuarantId",
                        column: x => x.RestuarantId,
                        principalTable: "Restuarants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationsAvailability_RestuarantId",
                table: "ReservationsAvailability",
                column: "RestuarantId");
        }
    }
}
