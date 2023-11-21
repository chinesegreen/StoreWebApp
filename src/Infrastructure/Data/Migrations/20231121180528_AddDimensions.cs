using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDimensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DimensionsId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Dimensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    Width = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    Lenght = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimensions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_DimensionsId",
                table: "Products",
                column: "DimensionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Dimensions_DimensionsId",
                table: "Products",
                column: "DimensionsId",
                principalTable: "Dimensions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Dimensions_DimensionsId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Dimensions");

            migrationBuilder.DropIndex(
                name: "IX_Products_DimensionsId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DimensionsId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Products");
        }
    }
}
