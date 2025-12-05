using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_CategoriesColor_ColorId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "CategoriesColor");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ColorId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoriesColor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesColor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ColorId",
                table: "Categories",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_CategoriesColor_ColorId",
                table: "Categories",
                column: "ColorId",
                principalTable: "CategoriesColor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
