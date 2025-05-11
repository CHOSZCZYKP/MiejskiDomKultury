using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilmId",
                table: "Uzytkownicy",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Filmy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActorsNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kinds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filmy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seanse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seanse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seanse_Filmy_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Filmy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seanse_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bilety",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeansId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bilety", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bilety_Seanse_SeansId",
                        column: x => x.SeansId,
                        principalTable: "Seanse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownicy_FilmId",
                table: "Uzytkownicy",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Bilety_SeansId",
                table: "Bilety",
                column: "SeansId");

            migrationBuilder.CreateIndex(
                name: "IX_Seanse_FilmId",
                table: "Seanse",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Seanse_UzytkownikId",
                table: "Seanse",
                column: "UzytkownikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uzytkownicy_Filmy_FilmId",
                table: "Uzytkownicy",
                column: "FilmId",
                principalTable: "Filmy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uzytkownicy_Filmy_FilmId",
                table: "Uzytkownicy");

            migrationBuilder.DropTable(
                name: "Bilety");

            migrationBuilder.DropTable(
                name: "Seanse");

            migrationBuilder.DropTable(
                name: "Filmy");

            migrationBuilder.DropIndex(
                name: "IX_Uzytkownicy_FilmId",
                table: "Uzytkownicy");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Uzytkownicy");
        }
    }
}
