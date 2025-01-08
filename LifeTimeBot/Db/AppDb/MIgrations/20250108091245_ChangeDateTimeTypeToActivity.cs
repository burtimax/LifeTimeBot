using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeTimeBot.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateTimeTypeToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "start_time",
                schema: "app",
                table: "activities",
                type: "timestamp without time zone",
                nullable: true,
                comment: "Время начала активности",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Время начала активности");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_time",
                schema: "app",
                table: "activities",
                type: "timestamp without time zone",
                nullable: true,
                comment: "Время окончания активности",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Время окончания активности");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "start_time",
                schema: "app",
                table: "activities",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Время начала активности",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldComment: "Время начала активности");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "end_time",
                schema: "app",
                table: "activities",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Время окончания активности",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldComment: "Время окончания активности");
        }
    }
}
