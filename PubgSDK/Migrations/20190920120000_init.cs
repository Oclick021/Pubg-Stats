using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PubgSDK.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    GameMode = table.Column<string>(nullable: true),
                    MapName = table.Column<string>(nullable: true),
                    IsCustomMatch = table.Column<bool>(nullable: false),
                    SeasonState = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantsStats",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    PlayerId = table.Column<string>(nullable: true),
                    DBNOs = table.Column<int>(nullable: false),
                    Boosts = table.Column<int>(nullable: false),
                    DeathType = table.Column<string>(nullable: true),
                    Heals = table.Column<int>(nullable: false),
                    KillPlace = table.Column<int>(nullable: false),
                    KillPointsDelta = table.Column<int>(nullable: false),
                    KillStreaks = table.Column<int>(nullable: false),
                    LastKillPoints = table.Column<int>(nullable: false),
                    LastWinPoints = table.Column<int>(nullable: false),
                    MostDamage = table.Column<int>(nullable: false),
                    Revives = table.Column<int>(nullable: false),
                    RideDistance = table.Column<float>(nullable: false),
                    RoadKills = table.Column<int>(nullable: false),
                    SwimDistance = table.Column<float>(nullable: false),
                    TeamKills = table.Column<int>(nullable: false),
                    TimeSurvived = table.Column<float>(nullable: false),
                    VehicleDestroys = table.Column<int>(nullable: false),
                    WalkDistance = table.Column<float>(nullable: false),
                    WeaponsAcquired = table.Column<int>(nullable: false),
                    WinPlace = table.Column<int>(nullable: false),
                    WinPointsDelta = table.Column<int>(nullable: false),
                    Assists = table.Column<int>(nullable: false),
                    DamageDealt = table.Column<float>(nullable: false),
                    HeadshotKills = table.Column<int>(nullable: false),
                    Kills = table.Column<int>(nullable: false),
                    LongestKill = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantsStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeasonStats",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    BestRankPoint = table.Column<float>(nullable: false),
                    Assists = table.Column<int>(nullable: false),
                    DailyKills = table.Column<int>(nullable: false),
                    DailyWins = table.Column<int>(nullable: false),
                    DamageDealt = table.Column<float>(nullable: false),
                    HeadshotKills = table.Column<int>(nullable: false),
                    Kills = table.Column<int>(nullable: false),
                    LongestKill = table.Column<float>(nullable: false),
                    MaxKillStreaks = table.Column<int>(nullable: false),
                    RankPoints = table.Column<float>(nullable: false),
                    RoundsPlayed = table.Column<int>(nullable: false),
                    Top10s = table.Column<int>(nullable: false),
                    WeeklyKills = table.Column<int>(nullable: false),
                    WeeklyWins = table.Column<int>(nullable: false),
                    Wins = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonStats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rosters",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Won = table.Column<bool>(nullable: false),
                    MatchId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rosters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rosters_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SoloStatsID = table.Column<string>(nullable: true),
                    DuoStatsID = table.Column<string>(nullable: true),
                    SquadStatsID = table.Column<string>(nullable: true),
                    CurrentSeasionLastUpdate = table.Column<DateTime>(nullable: true),
                    LastMatchUpdate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_SeasonStats_DuoStatsID",
                        column: x => x.DuoStatsID,
                        principalTable: "SeasonStats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_SeasonStats_SoloStatsID",
                        column: x => x.SoloStatsID,
                        principalTable: "SeasonStats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_SeasonStats_SquadStatsID",
                        column: x => x.SquadStatsID,
                        principalTable: "SeasonStats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    StatsID = table.Column<long>(nullable: false),
                    RosterId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Rosters_RosterId",
                        column: x => x.RosterId,
                        principalTable: "Rosters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participants_ParticipantsStats_StatsID",
                        column: x => x.StatsID,
                        principalTable: "ParticipantsStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMatch",
                columns: table => new
                {
                    PlayerId = table.Column<string>(nullable: false),
                    MatchId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMatch", x => new { x.PlayerId, x.MatchId });
                    table.ForeignKey(
                        name: "FK_PlayerMatch_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMatch_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_RosterId",
                table: "Participants",
                column: "RosterId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_StatsID",
                table: "Participants",
                column: "StatsID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatch_MatchId",
                table: "PlayerMatch",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_DuoStatsID",
                table: "Players",
                column: "DuoStatsID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_SoloStatsID",
                table: "Players",
                column: "SoloStatsID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_SquadStatsID",
                table: "Players",
                column: "SquadStatsID");

            migrationBuilder.CreateIndex(
                name: "IX_Rosters_MatchId",
                table: "Rosters",
                column: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "PlayerMatch");

            migrationBuilder.DropTable(
                name: "Rosters");

            migrationBuilder.DropTable(
                name: "ParticipantsStats");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "SeasonStats");
        }
    }
}
