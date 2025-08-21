using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShhhToshiApp.Migrations
{
    /// <inheritdoc />
    public partial class TaskRewardSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StakedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TONBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastStakedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletUsers", x => x.Id);
                    table.UniqueConstraint("AK_WalletUsers_WalletAddress", x => x.WalletAddress);
                });

            migrationBuilder.CreateTable(
                name: "PointClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PointsClaimed = table.Column<int>(type: "int", nullable: false),
                    ConvertedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointClaims_WalletUsers_WalletAddress",
                        column: x => x.WalletAddress,
                        principalTable: "WalletUsers",
                        principalColumn: "WalletAddress",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCompletions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCompletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskCompletions_TaskItems_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskCompletions_WalletUsers_WalletAddress",
                        column: x => x.WalletAddress,
                        principalTable: "WalletUsers",
                        principalColumn: "WalletAddress",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointClaims_WalletAddress",
                table: "PointClaims",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCompletions_TaskId",
                table: "TaskCompletions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCompletions_WalletAddress",
                table: "TaskCompletions",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_WalletUsers_WalletAddress",
                table: "WalletUsers",
                column: "WalletAddress",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointClaims");

            migrationBuilder.DropTable(
                name: "TaskCompletions");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "WalletUsers");
        }
    }
}
