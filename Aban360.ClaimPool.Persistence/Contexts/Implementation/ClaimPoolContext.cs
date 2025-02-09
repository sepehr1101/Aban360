using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Contexts.Implementation
{
    public partial class ClaimPoolContext : BaseDbContext,IUnitOfWork
    {
        public ClaimPoolContext()
        {
        }

        public ClaimPoolContext(DbContextOptions<ClaimPoolContext> options)
            : base(options)
        {
        }       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //var connectionString = MigrationRunner.GetConnectionInfo().Item1;
                //optionsBuilder.UseSqlServer(connectionString,
                //        serverDbContextOptionsBuilder =>
                //        {
                //            var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                //            serverDbContextOptionsBuilder.CommandTimeout(minutes);
                //            //serverDbContextOptionsBuilder.EnableRetryOnFailure();
                //        });
                //optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor());
                //optionsBuilder.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}