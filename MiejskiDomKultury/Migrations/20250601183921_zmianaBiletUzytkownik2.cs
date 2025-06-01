using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class zmianaBiletUzytkownik2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilety_Uzytkownicy_UserId",
                table: "Bilety");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilety_Uzytkownicy_UserId",
                table: "Bilety",
                column: "UserId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilety_Uzytkownicy_UserId",
                table: "Bilety");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilety_Uzytkownicy_UserId",
                table: "Bilety",
                column: "UserId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
