namespace LifeTimeBot.Db.AppDb.Entities;

public enum UserTaskType
{
    /// <summary>
    /// Другое
    /// </summary>
    None = 0,
    /// <summary>
    /// Задача на один день
    /// </summary>
    OneDay = 1,
    /// <summary>
    /// Задача на неделю
    /// </summary>
    OneWeek = 7,
}