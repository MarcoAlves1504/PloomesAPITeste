using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PloomesAPITeste.Migrations
{
    /// <inheritdoc />
    public partial class AllowEmptyChannels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextChannelUserAccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
