using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballForAll.Data.Migrations
{
    public partial class RenameSeasonTableToTeamPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeasonTables");

            migrationBuilder.CreateTable(
                name: "TeamPositions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    SeasonId = table.Column<int>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    Points = table.Column<byte>(nullable: false),
                    Won = table.Column<byte>(nullable: false),
                    Drawn = table.Column<byte>(nullable: false),
                    Lost = table.Column<byte>(nullable: false),
                    GoalsFor = table.Column<byte>(nullable: false),
                    GoalsAgainst = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamPositions_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamPositions_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamPositions_ClubId",
                table: "TeamPositions",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPositions_SeasonId",
                table: "TeamPositions",
                column: "SeasonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPositions");

            migrationBuilder.CreateTable(
                name: "SeasonTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Drawn = table.Column<byte>(type: "tinyint", nullable: false),
                    GoalsAgainst = table.Column<byte>(type: "tinyint", nullable: false),
                    GoalsFor = table.Column<byte>(type: "tinyint", nullable: false),
                    Lost = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Points = table.Column<byte>(type: "tinyint", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    Won = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeasonTables_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeasonTables_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTables_ClubId",
                table: "SeasonTables",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTables_SeasonId",
                table: "SeasonTables",
                column: "SeasonId");
        }
    }
}
