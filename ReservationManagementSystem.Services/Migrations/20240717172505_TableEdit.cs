using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManagementSystem.Services.Migrations
{
    /// <inheritdoc />
    public partial class TableEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartySize",
                table: "Tables",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartySize",
                table: "Tables");
        }
    }
}
