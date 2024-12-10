using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeTimeBot.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class AddTypesToActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "balance_types",
                schema: "app",
                table: "activities",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0],
                comment: "Сферы жизненного баланса активности.");

            migrationBuilder.AddColumn<string>(
                name: "comment",
                schema: "app",
                table: "activities",
                type: "text",
                nullable: true,
                comment: "Комментарий к активности, подробности от пользователя.");

            migrationBuilder.AddColumn<int>(
                name: "type",
                schema: "app",
                table: "activities",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Тип активности.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "balance_types",
                schema: "app",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "comment",
                schema: "app",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "app",
                table: "activities");
        }
    }
}
