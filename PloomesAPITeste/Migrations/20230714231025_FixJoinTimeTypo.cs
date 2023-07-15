using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PloomesAPITeste.Migrations
{
    /// <inheritdoc />
    public partial class FixJoinTimeTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "joinTime",
                table: "UserChannelJoinsLog",
                newName: "JoinTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoinTime",
                table: "UserChannelJoinsLog",
                newName: "joinTime");
        }
    }
}
