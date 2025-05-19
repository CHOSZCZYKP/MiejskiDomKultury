using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ogloszenia",
                columns: new[] { "Id", "Content", "CreatedAt", "FileName", "Title" },
                values: new object[,]
                {
                    { 1, "Astrolodzy przewidują opad meteorytów dzisiaj o 21:00!", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assets/news1.webp", "Astrolodzy przewidują opad meteorytów!" },
                    { 2, "Kobieta poszukiwana za zdemolowanie sali tanecznej!", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assets/news2.webp", "Zdemolowanie sali tanecznej" },
                    { 3, "stereotyp o podłożu antysemickim wynikający ze spiskowej teorii dziejów, przypisujący Żydom główną rolę w stworzeniu i spopularyzowaniu ideologii komunizmu, który „miał otworzyć im drogę do zdobycia władzy nad światem”", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assets/news3.webp", "Ostrołęka pogrążona w żałobie. Zmarł Bóbr Bartek" },
                    { 4, "Joanna Senyszyn opisała także, że jest jedyną przedstawicielką lewicy w tych wyborach, bo Adrian Zandberg to \"skrajna lewica\". — Magdalena Biejat to z kolei żadna lewica.", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assets/news4.webp", "Parada Imperatora w Ostrołęce - zdjęcia" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ogloszenia",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ogloszenia",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ogloszenia",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ogloszenia",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
