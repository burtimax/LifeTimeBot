using LifeTimeBot.Db.AppDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeTimeBot.Db.AppDb;

public partial class AppDbContext : DbContext
{
    private const string appSchema = "app";
    
    protected string? InitiatorUserId { get; set; }

    // public AppDbContext() { }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //  //Определение провайдера необходимо для создания миграции, поэтому пусть пока побудет здесь.
    //  string mockString = "Host=127.0.0.1;Port=5432;Database=life_time_bot_db;Username=postgres;Password=123;Include Error Detail=true";
    //  optionsBuilder.UseNpgsql(mockString);
    //  base.OnConfiguring(optionsBuilder);
    // }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options, IServiceProvider serviceProvider) : base(options)
    {
    }
    
    // Коллекции данных
    public DbSet<ActivityEntity> Activity => Set<ActivityEntity>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        SetSchemasToTables(builder);
        SetAllToSnakeCase(builder);
        SetFilters(builder);
        ConfigureEntities(builder);
        base.OnModelCreating(builder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var e in
                 ChangeTracker.Entries<IBaseEntity>())
        {
            switch (e.State)
            {
                case EntityState.Added:
                    e.Entity.CreatedAt = DateTimeOffset.Now;
                    e.Entity.CreatedBy = InitiatorUserId;
                    break;
                case EntityState.Modified:
                    e.Entity.UpdatedAt = DateTimeOffset.Now;
                    e.Entity.UpdatedBy = InitiatorUserId;
                    break;
                case EntityState.Deleted:
                    e.Entity.DeletedAt = DateTimeOffset.Now;
                    e.Entity.DeletedBy = InitiatorUserId;
                    e.State = EntityState.Modified;
                    break;
            }
        }

        return base.SaveChangesAsync(ct);
    }
    
}
