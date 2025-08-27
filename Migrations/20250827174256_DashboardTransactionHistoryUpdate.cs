using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShhhToshiApp.Migrations
{
    /// <inheritdoc />
    public partial class DashboardTransactionHistoryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StakeUnstakeEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StakeUnstakeEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StakeUnstakeEvents_WalletUsers_WalletAddress",
                        column: x => x.WalletAddress,
                        principalTable: "WalletUsers",
                        principalColumn: "WalletAddress",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StakeUnstakeEvents_WalletAddress",
                table: "StakeUnstakeEvents",
                column: "WalletAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StakeUnstakeEvents");
        }
    }
}
