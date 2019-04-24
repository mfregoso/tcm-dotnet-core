using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TCM.Web.Migrations
{
    public partial class AddExpirationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HistoryExpiration",
                table: "Clubs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TMIExpiration",
                table: "Clubs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HistoryExpiration",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "TMIExpiration",
                table: "Clubs");
        }
    }
}
