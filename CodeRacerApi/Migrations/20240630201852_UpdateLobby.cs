using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeRacerApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLobby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Lobbys",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Snippet",
                table: "Lobbys",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Lobbys");

            migrationBuilder.DropColumn(
                name: "Snippet",
                table: "Lobbys");
        }
    }
}
