using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class mig14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Czas",
                table: "Seanse");

            migrationBuilder.AddColumn<int>(
                name: "Czas",
                table: "Filmy",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Czas",
                table: "Filmy");

            migrationBuilder.AddColumn<int>(
                name: "Czas",
                table: "Seanse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
