using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PubgStatsBot.Migrations
{
    public partial class lastmatchnotifiedadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastMatchNotified",
                table: "Player",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMatchNotified",
                table: "Player");
        }
    }
}
