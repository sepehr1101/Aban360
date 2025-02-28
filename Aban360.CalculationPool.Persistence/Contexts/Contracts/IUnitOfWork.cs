using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Aban360.CalculationPool.Persistence.Contexts.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        DatabaseFacade GetDatabase();
        EntityEntry GetEntry<T>(T entity);
        EntityState GetEntityState(object entity);
        EntityState SetEntityState(object entity, EntityState entityState);
        Task<ICollection<T>> ExecuteQuery<T>(string rawSql, params object[] parameters) where T : class;
        Task<int> ExecuteNonResultQuery(string rawQuery, params string[] parameters);
        Task BulkInsert<T>(IList<T> entities) where T : class;
        Task BulkInsert<T1, T2>(IList<T1> entities1, IList<T2> entities2) where T1 : class where T2 : class;

        TEntity FindOrThrow<TEntity>(params object?[]? keyValues) where TEntity : class;
        Task<TEntity> FindOrThrowAsync<TEntity>(params object?[]? keyValues) where TEntity : class;
        void ExecuteBatch(string sqlFilePath);
    }
}
