using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using EFCore.BulkExtensions;
using Aban360.Common.Extensions;
using System.Transactions;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.Exceptions;

namespace Aban360.ClaimPool.Persistence.Contexts.Implementation
{
    public abstract class BaseDbContext : DbContext, IUnitOfWork
    {
        public BaseDbContext()
        {

        }
        public BaseDbContext(DbContextOptions options)
          : base(options)
        {
        }
        public DatabaseFacade GetDatabase()
        {
            return Database;
        }
        public EntityEntry GetEntry<T>(T entity)
        {
            return Entry(entity);
        }
        public EntityState GetEntityState(object entity)
        {
            return Entry(entity).State;
        }
        public EntityState SetEntityState(object entity, EntityState entityState)
        {
            return Entry(entity).State = entityState;
        }
        public async Task<ICollection<T>> ExecuteQuery<T>(string sql, params object[] parameters) where T : class
        {
            using (var db2 = new ContextForQueryType<T>(GetDatabase().GetDbConnection()))
            {
                return await db2.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
            }
        }
        public async Task<int> ExecuteNonResultQuery(string rawQuery, params string[] parameters)
        {
            return await GetDatabase().ExecuteSqlRawAsync(rawQuery, parameters);
        }
        public async Task BulkInsert<T>(IList<T> entities) where T : class
        {
            var bulkConfig = new BulkConfig() { BulkCopyTimeout = int.MaxValue };
            await this.BulkInsertAsync(entities, bulkConfig);
        }
        public async Task BulkInsert<T1, T2>(IList<T1> entities1, IList<T2> entities2) where T1 : class where T2 : class
        {
            using (var transaction = TransactionBuilder.Create(2, 0, IsolationLevel.ReadUncommitted))
            {
                await BulkInsert(entities1);
                await BulkInsert(entities2);
                transaction.Complete();
            }
        }
        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            return result;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync();
            return result;
        }
        public TEntity FindOrThrow<TEntity>(params object?[]? keyValues)
            where TEntity : class
        {
            var entity = Find<TEntity>(keyValues);
            if (entity == null)
            {
                throw new InvalidIdException();
            }
            return entity;
        }
        public async Task<TEntity> FindOrThrowAsync<TEntity>(params object?[]? keyValues)
          where TEntity : class
        {
            var entity = await FindAsync<TEntity>(keyValues);
            if (entity is null)
            {
                throw new InvalidIdException();
            }
            return entity;
        }

        class ContextForQueryType<T> : DbContext where T : class
        {
            private readonly DbConnection _connection;
            public ContextForQueryType(DbConnection connection)
            {
                _connection = connection;
            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(_connection, options => options.EnableRetryOnFailure());

                base.OnConfiguring(optionsBuilder);
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>().HasNoKey();
                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
