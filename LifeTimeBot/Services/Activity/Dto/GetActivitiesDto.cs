using MultipleBotFrameworkEndpoints.Models;

namespace LifeTimeBot.Services.Dto;

public class GetActivitiesDto: IOrdered
{
    public List<long>? Ids { get; set; }
    public List<long>? BotIds { get; set; }
    public List<long>? ChatIds { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public bool? Confirmed { get; set; }
    public string? Order { get; set; }
    
    /// <summary>
    /// Нужно ли включать те активности которые только частично попадают в интервал.
    /// False - включаем
    /// True - не включаем
    /// </summary>
    public bool StrictRestrictions { get; set; } = false;
    
    /// <summary>
    /// Нужно ли обрезать даты для актисностей, которые попадают частично.
    /// </summary>
    public bool CropDates { get; set; } = false;
    
}