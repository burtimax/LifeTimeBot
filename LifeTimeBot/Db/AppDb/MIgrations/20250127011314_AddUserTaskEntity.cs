using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LifeTimeBot.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTaskEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_tasks",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "ИД сущности.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bot_id = table.Column<long>(type: "bigint", nullable: false, comment: "ИД бота"),
                    telegram_chat_id = table.Column<long>(type: "bigint", nullable: false, comment: "ИД чата, владельца активности"),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Время начала задачи"),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Время окончания задачи"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "Описание задачи"),
                    comment = table.Column<string>(type: "text", nullable: true, comment: "Комментарий к задаче, подробности от пользователя"),
                    emoji = table.Column<string>(type: "text", nullable: true, comment: "Emoji задачи"),
                    audio_file_id = table.Column<string>(type: "text", nullable: true, comment: "FileId голосового"),
                    message_id = table.Column<int>(type: "integer", nullable: true, comment: "ИД сообщения"),
                    message_text = table.Column<string>(type: "text", nullable: true, comment: "Текст аудиосообщения"),
                    confirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "Подтверждена задача пользователем? Правильно ли сформирована."),
                    type = table.Column<int>(type: "integer", nullable: false, comment: "Тип задачи."),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Когда сущность была создана."),
                    created_by = table.Column<string>(type: "text", nullable: true, comment: "Кто создал сущность."),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Когда сущность была в последний раз обновлена."),
                    updated_by = table.Column<string>(type: "text", nullable: true, comment: "Кто обновил сущность."),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Когда сущность была удалена."),
                    deleted_by = table.Column<string>(type: "text", nullable: true, comment: "Кто удалил сущность.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tasks", x => x.id);
                },
                comment: "Задачи пользователей");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_tasks",
                schema: "app");
        }
    }
}
