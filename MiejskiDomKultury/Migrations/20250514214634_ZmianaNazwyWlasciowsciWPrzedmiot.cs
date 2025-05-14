using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class ZmianaNazwyWlasciowsciWPrzedmiot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CenaZaDobe_Wartość",
                table: "Przedmioty",
                newName: "CenaZaDobe_Wartosc");

            migrationBuilder.AlterColumn<string>(
                name: "Aktorzy",
                table: "Filmy",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CenaZaDobe_Wartosc",
                table: "Przedmioty",
                newName: "CenaZaDobe_Wartość");

            migrationBuilder.AlterColumn<string>(
                name: "Aktorzy",
                table: "Filmy",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
