using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManagementSystem.Services.Migrations
{
    /// <inheritdoc />
    public partial class Adminupdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Restuarants_RestuarantId",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Admins_RestuarantId",
                table: "Admins");

            migrationBuilder.AlterColumn<int>(
                name: "RestuarantId",
                table: "Admins",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_RestuarantId",
                table: "Admins",
                column: "RestuarantId",
                unique: true,
                filter: "[RestuarantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Restuarants_RestuarantId",
                table: "Admins",
                column: "RestuarantId",
                principalTable: "Restuarants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Restuarants_RestuarantId",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Admins_RestuarantId",
                table: "Admins");

            migrationBuilder.AlterColumn<int>(
                name: "RestuarantId",
                table: "Admins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
        }
    }
}
