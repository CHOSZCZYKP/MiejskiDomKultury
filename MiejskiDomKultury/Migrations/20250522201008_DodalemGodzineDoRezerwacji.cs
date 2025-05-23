using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiejskiDomKultury.Migrations
{
    /// <inheritdoc />
    public partial class DodalemGodzineDoRezerwacji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoKiedy",
                table: "Rezerwacje");

            migrationBuilder.RenameColumn(
                name: "OdKiedy",
                table: "Rezerwacje",
                newName: "Data");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "GodzinaKoncowa",
                table: "Rezerwacje",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "GodzinaPoczatkowa",
                table: "Rezerwacje",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GodzinaKoncowa",
                table: "Rezerwacje");

            migrationBuilder.DropColumn(
                name: "GodzinaPoczatkowa",
                table: "Rezerwacje");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Rezerwacje",
                newName: "OdKiedy");

            migrationBuilder.AddColumn<DateTime>(
                name: "DoKiedy",
                table: "Rezerwacje",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
