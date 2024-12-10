﻿using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace LifeTimeBot.Db;

/// <summary>
/// Базовая сущность БД.
/// </summary>
public class BaseEntity<TKey> : IBaseEntity
{
    /// <summary>
    /// ИД сущности.
    /// </summary>
    [Comment("ИД сущности.")]
    public TKey Id { get; set; }
    
    /// <summary>
    /// Когда сущность была создана.
    /// </summary>
    [Comment("Когда сущность была создана.")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кто создал сущность.
    /// </summary>
    [Comment("Кто создал сущность.")]
    [JsonIgnore]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Когда сущность была в последний раз обновлена.
    /// </summary>
    [Comment("Когда сущность была в последний раз обновлена.")]
    [JsonIgnore]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Кто обновил сущность.
    /// </summary>
    [Comment("Кто обновил сущность.")]
    [JsonIgnore]
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Когда сущность была удалена.
    /// </summary>
    [Comment("Когда сущность была удалена.")]
    [JsonIgnore]
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Кто удалил сущность.
    /// </summary>
    [Comment("Кто удалил сущность.")]
    [JsonIgnore]
    public string? DeletedBy { get; set; }
}