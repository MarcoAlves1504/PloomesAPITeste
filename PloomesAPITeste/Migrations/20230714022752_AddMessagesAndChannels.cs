using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PloomesAPITeste.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagesAndChannels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ChannelDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextMessages",
                columns: table => new
                {
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageSentTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextMessages", x => new { x.SenderId, x.ChannelId });
                });

            migrationBuilder.CreateTable(
                name: "TextChannelUserAccount",
                columns: table => new
                {
                    ChannelMembersId = table.Column<int>(type: "int", nullable: false),
                    ChannelsJoinedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextChannelUserAccount", x => new { x.ChannelMembersId, x.ChannelsJoinedId });
                    table.ForeignKey(
                        name: "FK_TextChannelUserAccount_TextChannels_ChannelsJoinedId",
                        column: x => x.ChannelsJoinedId,
                        principalTable: "TextChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextChannelUserAccount_UserAccounts_ChannelMembersId",
                        column: x => x.ChannelMembersId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextChannelUserAccount_ChannelsJoinedId",
                table: "TextChannelUserAccount",
                column: "ChannelsJoinedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextChannelUserAccount");

            migrationBuilder.DropTable(
                name: "TextMessages");

            migrationBuilder.DropTable(
                name: "TextChannels");
        }
    }
}
