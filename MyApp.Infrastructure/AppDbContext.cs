using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace MyApp.Infrastructure
{
    public class AppDbContext : DbContext, IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly Dictionary<Type, object> _repositories = [];
        private IDbContextTransaction? _currentTransaction;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                var repo = new GenericRepository<TEntity>(this);
                _repositories.Add(typeof(TEntity), repo);
            }
            return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        public async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();

        // Transaction Methods
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction == null)
                _currentTransaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public override void Dispose()
        {
            _currentTransaction?.Dispose();
            base.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
                await _currentTransaction.DisposeAsync();

            await base.DisposeAsync();
        }

    }
}