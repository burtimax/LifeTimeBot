using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeTimeBot.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class AddEmojiToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "emoji",
                schema: "app",
                table: "activities",
                type: "text",
                nullable: true,
                comment: "Emoji активности");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emoji",
                schema: "app",
                table: "activities");
        }
    }
}
