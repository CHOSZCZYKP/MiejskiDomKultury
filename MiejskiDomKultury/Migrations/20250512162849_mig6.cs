using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seanse_Uzytkownicy_UzytkownikId",
                table: "Seanse");

            migrationBuilder.DropForeignKey(
                name: "FK_Uzytkownicy_Filmy_FilmId",
                table: "Uzytkownicy");

            migrationBuilder.DropIndex(
                name: "IX_Uzytkownicy_FilmId",
                table: "Uzytkownicy");

            migrationBuilder.DropIndex(
                name: "IX_Seanse_UzytkownikId",
                table: "Seanse");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Uzytkownicy");

            migrationBuilder.DropColumn(
                name: "UzytkownikId",
                table: "Seanse");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieShowUzytkownik");

            migrationBuilder.AddColumn<int>(
                name: "FilmId",
                table: "Uzytkownicy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UzytkownikId",
                table: "Seanse",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownicy_FilmId",
                table: "Uzytkownicy",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Seanse_UzytkownikId",
                table: "Seanse",
                column: "UzytkownikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seanse_Uzytkownicy_UzytkownikId",
                table: "Seanse",
                column: "UzytkownikId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Uzytkownicy_Filmy_FilmId",
                table: "Uzytkownicy",
                column: "FilmId",
                principalTable: "Filmy",
                principalColumn: "Id");
        }
    }
}
