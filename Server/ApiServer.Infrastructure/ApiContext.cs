using ApiServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Infrastructure;

public class ApiContext : DbContext
{
    #region Constructor
    
    //Default Constructor
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    { }
    
    #endregion
    
    #region Db Set Declaration
    
    public DbSet<User> Users { get; set; }
    public DbSet<FlashcardSet> FlashcardSets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<ApiSetting> ApiSettings { get; set; }
    public DbSet<TelemetrySession> TelemetrySessions { get; set; }
    
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}