using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManagementSystem.Services.Migrations
{
    /// <inheritdoc />
    public partial class AvailabilityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Duration",
                table: "Admins",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Duration",
                table: "Admins",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);
        }
    }
}
