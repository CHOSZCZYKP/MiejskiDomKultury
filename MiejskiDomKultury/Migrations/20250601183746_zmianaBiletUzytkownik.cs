using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class zmianaBiletUzytkownik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeansUzytkownik");

            migrationBuilder.AddColumn<int>(
                name: "SeansId",
                table: "Uzytkownicy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Bilety",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownicy_SeansId",
                table: "Uzytkownicy",
                column: "SeansId");

            migrationBuilder.CreateIndex(
                name: "IX_Bilety_UserId",
                table: "Bilety",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilety_Uzytkownicy_UserId",
                table: "Bilety",
                column: "UserId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Uzytkownicy_Seanse_SeansId",
                table: "Uzytkownicy",
                column: "SeansId",
                principalTable: "Seanse",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilety_Uzytkownicy_UserId",
                table: "Bilety");

            migrationBuilder.DropForeignKey(
                name: "FK_Uzytkownicy_Seanse_SeansId",
                table: "Uzytkownicy");

            migrationBuilder.DropIndex(
                name: "IX_Uzytkownicy_SeansId",
                table: "Uzytkownicy");

            migrationBuilder.DropIndex(
                name: "IX_Bilety_UserId",
                table: "Bilety");

            migrationBuilder.DropColumn(
                name: "SeansId",
                table: "Uzytkownicy");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bilety");

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
    }
}
