using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManagementSystem.Services.Migrations
{
    /// <inheritdoc />
    public partial class Restuarantandrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationsAvailability_Admins_AdminId",
                table: "ReservationsAvailability");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "ReservationsAvailability",
                newName: "RestuarantId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationsAvailability_AdminId",
                table: "ReservationsAvailability",
                newName: "IX_ReservationsAvailability_RestuarantId");

            migrationBuilder.AddColumn<int>(
                name: "RestuarantId",
                table: "Admins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Restuarants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanySite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Menu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinimumSpend = table.Column<int>(type: "int", nullable: true),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restuarants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_RestuarantId",
                table: "Admins",
                column: "RestuarantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Restuarants_RestuarantId",
                table: "Admins",
                column: "RestuarantId",
                principalTable: "Restuarants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationsAvailability_Restuarants_RestuarantId",
                table: "ReservationsAvailability",
                column: "RestuarantId",
                principalTable: "Restuarants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Restuarants_RestuarantId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationsAvailability_Restuarants_RestuarantId",
                table: "ReservationsAvailability");

            migrationBuilder.DropTable(
                name: "Restuarants");

            migrationBuilder.DropIndex(
                name: "IX_Admins_RestuarantId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "RestuarantId",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "RestuarantId",
                table: "ReservationsAvailability",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationsAvailability_RestuarantId",
                table: "ReservationsAvailability",
                newName: "IX_ReservationsAvailability_AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationsAvailability_Admins_AdminId",
                table: "ReservationsAvailability",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
