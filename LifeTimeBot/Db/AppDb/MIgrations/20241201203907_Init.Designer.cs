﻿// <auto-generated />
using System;
using LifeTimeBot.Db.AppDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LifeTimeBot.Db.AppDb.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241201203907_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LifeTimeBot.Db.AppDb.Entities.ActivityEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasComment("ИД сущности.");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AudioFileId")
                        .HasColumnType("text")
                        .HasColumnName("audio_file_id")
                        .HasComment("FileId голосового");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("confirmed")
                        .HasComment("Подтверждена активность пользователем? Правильно ли сформирована.");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasComment("Когда сущность была создана.");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by")
                        .HasComment("Кто создал сущность.");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at")
                        .HasComment("Когда сущность была удалена.");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text")
                        .HasColumnName("deleted_by")
                        .HasComment("Кто удалил сущность.");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description")
                        .HasComment("Описание активности");

                    b.Property<DateTimeOffset?>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time")
                        .HasComment("Время окончания активности");

                    b.Property<int?>("MessageId")
                        .HasColumnType("integer")
                        .HasColumnName("message_id")
                        .HasComment("ИД сообщения");

                    b.Property<string>("MessageText")
                        .HasColumnType("text")
                        .HasColumnName("message_text")
                        .HasComment("Текст аудиосообщения");

                    b.Property<DateTimeOffset?>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time")
                        .HasComment("Время начала активности");

                    b.Property<long>("TelegramChatId")
                        .HasColumnType("bigint")
                        .HasColumnName("telegram_chat_id")
                        .HasComment("ИД чата, владельца активности");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasComment("Когда сущность была в последний раз обновлена.");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by")
                        .HasComment("Кто обновил сущность.");

                    b.HasKey("Id")
                        .HasName("pk_activities");

                    b.ToTable("activities", "app", t =>
                        {
                            t.HasComment("Активности пользователей");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
