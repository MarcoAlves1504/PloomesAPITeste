using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PloomesAPITeste.Migrations
{
    /// <inheritdoc />
    public partial class FixMessagesPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TextMessages",
                table: "TextMessages");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "TextMessages",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextMessages",
                table: "TextMessages",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TextMessages",
                table: "TextMessages");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "TextMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextMessages",
                table: "TextMessages",
                columns: new[] { "SenderId", "ChannelId" });
        }
    }
}
