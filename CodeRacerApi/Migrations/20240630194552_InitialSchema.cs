using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeRacerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lobbys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LobbyName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobbys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LobbyUser",
                columns: table => new
                {
                    LobbysId = table.Column<string>(type: "TEXT", nullable: false),
                    UsersId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyUser", x => new { x.LobbysId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_LobbyUser_Lobbys_LobbysId",
                        column: x => x.LobbysId,
                        principalTable: "Lobbys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LobbyUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LobbyUser_UsersId",
                table: "LobbyUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LobbyUser");

            migrationBuilder.DropTable(
                name: "Lobbys");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
