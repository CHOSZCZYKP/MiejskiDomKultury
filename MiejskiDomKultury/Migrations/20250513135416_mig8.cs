using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class mig8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieShowUzytkownik");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Seanse",
                newName: "Czas");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                table: "Seanse",
                newName: "DataStart");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Filmy",
                newName: "Rok");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Filmy",
                newName: "Tytul");

            migrationBuilder.RenameColumn(
                name: "Kinds",
                table: "Filmy",
                newName: "PlakatURL");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Filmy",
                newName: "Opis");

            migrationBuilder.RenameColumn(
                name: "ActorsNames",
                table: "Filmy",
                newName: "Gatunki");

            migrationBuilder.AddColumn<string>(
                name: "Aktorzy",
                table: "Filmy",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateTable(
                name: "SeansUzytkownik",
                columns: table => new
                {
                    SeanseId = table.Column<int>(type: "int", nullable: false),
                    WidzowieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeansUzytkownik", x => new { x.SeanseId, x.WidzowieId });
                    table.ForeignKey(
                        name: "FK_SeansUzytkownik_Seanse_SeanseId",
                        column: x => x.SeanseId,
                        principalTable: "Seanse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeansUzytkownik_Uzytkownicy_WidzowieId",
                        column: x => x.WidzowieId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeansUzytkownik_WidzowieId",
                table: "SeansUzytkownik",
                column: "WidzowieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeansUzytkownik");

            migrationBuilder.DropColumn(
                name: "Aktorzy",
                table: "Filmy");

            migrationBuilder.RenameColumn(
                name: "DataStart",
                table: "Seanse",
                newName: "DateStart");

            migrationBuilder.RenameColumn(
                name: "Czas",
                table: "Seanse",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "Tytul",
                table: "Filmy",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Rok",
                table: "Filmy",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "PlakatURL",
                table: "Filmy",
                newName: "Kinds");

            migrationBuilder.RenameColumn(
                name: "Opis",
                table: "Filmy",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Gatunki",
                table: "Filmy",
                newName: "ActorsNames");

            migrationBuilder.CreateTable(
                name: "MovieShowUzytkownik",
                columns: table => new
                {
                    SeanseId = table.Column<int>(type: "int", nullable: false),
                    WidzowieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieShowUzytkownik", x => new { x.SeanseId, x.WidzowieId });
                    table.ForeignKey(
                        name: "FK_MovieShowUzytkownik_Seanse_SeanseId",
                        column: x => x.SeanseId,
                        principalTable: "Seanse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieShowUzytkownik_Uzytkownicy_WidzowieId",
                        column: x => x.WidzowieId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowUzytkownik_WidzowieId",
                table: "MovieShowUzytkownik",
                column: "WidzowieId");
        }
    }
}
