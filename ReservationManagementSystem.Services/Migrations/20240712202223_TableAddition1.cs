using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManagementSystem.Services.Migrations
{
    /// <inheritdoc />
    public partial class TableAddition1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RestuarantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_Tables_Restuarants_RestuarantId",
                        column: x => x.RestuarantId,
                        principalTable: "Restuarants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tables_RestuarantId",
                table: "Tables",
                column: "RestuarantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
