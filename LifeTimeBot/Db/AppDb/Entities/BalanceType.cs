namespace LifeTimeBot.Db.AppDb.Entities;

/// <summary>
/// 8 Сфер жизненного баланса.
/// </summary>
public enum BalanceType
{
    /// <summary>
    /// Другое
    /// </summary>
    None = 0,
    /// <summary>
    /// Карьера/Бизнес
    /// </summary>
    СareerWorkBusiness = 1,
    /// <summary>
    /// Любовь/семья/дети
    /// </summary>
    LoveFamilyChildren = 2,
    /// <summary>
    /// Здоровье/спорт
    /// </summary>
    HealthyAndSport = 3,
    /// <summary>
    /// Друзья/окружение
    /// </summary>
    FriendsAndCommunity = 4,
    /// <summary>
    /// Хобби/развлечения/яркость жизни
    /// </summary>
    HobbyAndBrightnessOfLife = 5,
    /// <summary>
    /// Личностное развитие/образование
    /// </summary>
    PersonalDevelopmentAndEducation = 6,
    /// <summary>
    /// Финансы/сбережения/инвестиции
    /// </summary>
    Finance = 7,
    /// <summary>
    /// Духовность
    /// </summary>
    Spirituality = 8,
}