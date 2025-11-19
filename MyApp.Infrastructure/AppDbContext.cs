using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;

namespace MyApp.Infrastructure;

public class ControlDbContext : UnitOfWorkDbContext<ControlDbContext>, IControlUnitOfWork, IDisposable, IAsyncDisposable
{
    public ControlDbContext(DbContextOptions<ControlDbContext> options) : base(options) { }

    // implement OnModelCreating to configure entities  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ControlDbContext).Assembly);
    }
}

public class AppDbContext : UnitOfWorkDbContext<AppDbContext>, IAppUnitOfWork, IDisposable, IAsyncDisposable
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // implement OnModelCreating to configure entities  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}