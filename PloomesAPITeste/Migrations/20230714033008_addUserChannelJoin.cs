using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PloomesAPITeste.Migrations
{
    /// <inheritdoc />
    public partial class addUserChannelJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserChannelJoins",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    JoinTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChannelJoins", x => new { x.UserId, x.ChannelId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChannelJoins");
        }
    }
}
