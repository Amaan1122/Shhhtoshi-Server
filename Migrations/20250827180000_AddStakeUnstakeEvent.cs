using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShhhToshiApp.Migrations
{
    public partial class AddStakeUnstakeEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StakeUnstakeEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WalletAddress = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StakeUnstakeEvents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "StakeUnstakeEvents");
        }
    }
}
