using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class kinoUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cena",
                table: "Bilety",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cena",
                table: "Bilety");
        }
    }
}
