using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LifeTimeBot.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTaskNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_task_notifications",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "ИД сущности.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_task_id = table.Column<long>(type: "bigint", nullable: false, comment: "Внешний ключ на задачу пользователя"),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Когда сущность была создана."),
                    created_by = table.Column<string>(type: "text", nullable: true, comment: "Кто создал сущность."),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Когда сущность была в последний раз обновлена."),
                    updated_by = table.Column<string>(type: "text", nullable: true, comment: "Кто обновил сущность."),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Когда сущность была удалена."),
                    deleted_by = table.Column<string>(type: "text", nullable: true, comment: "Кто удалил сущность."),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "Статус уведомления"),
                    notification_date_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата и время уведомления"),
                    utc = table.Column<int>(type: "integer", nullable: false, comment: "Часовой пояс в формате UTC"),
                    bot_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор бота"),
                    telegram_chat_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор чата в Telegram"),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false, comment: "Флаг активности уведомления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_task_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_task_notifications_user_tasks_user_task_id",
                        column: x => x.user_task_id,
                        principalSchema: "app",
                        principalTable: "user_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_task_notifications_user_task_id",
                schema: "app",
                table: "user_task_notifications",
                column: "user_task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_task_notifications",
                schema: "app");
        }
    }
}
