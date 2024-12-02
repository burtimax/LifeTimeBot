using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LifeTimeBot.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "activities",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "ИД сущности.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    telegram_chat_id = table.Column<long>(type: "bigint", nullable: false, comment: "ИД чата, владельца активности"),
                    start_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Время начала активности"),
                    end_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Время окончания активности"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "Описание активности"),
                    audio_file_id = table.Column<string>(type: "text", nullable: true, comment: "FileId голосового"),
                    message_id = table.Column<int>(type: "integer", nullable: true, comment: "ИД сообщения"),
                    message_text = table.Column<string>(type: "text", nullable: true, comment: "Текст аудиосообщения"),
                    confirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "Подтверждена активность пользователем? Правильно ли сформирована."),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Когда сущность была создана."),
                    created_by = table.Column<string>(type: "text", nullable: true, comment: "Кто создал сущность."),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Когда сущность была в последний раз обновлена."),
                    updated_by = table.Column<string>(type: "text", nullable: true, comment: "Кто обновил сущность."),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Когда сущность была удалена."),
                    deleted_by = table.Column<string>(type: "text", nullable: true, comment: "Кто удалил сущность.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_activities", x => x.id);
                },
                comment: "Активности пользователей");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activities",
                schema: "app");
        }
    }
}
