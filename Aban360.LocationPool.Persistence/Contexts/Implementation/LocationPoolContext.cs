using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Contexts.Implementation;
using Aban360.LocationPool.Persistence.Extensions;
using Aban360.LocationPool.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aban360.UserPool.Persistence.Contexts.Implementation
{
    public partial class LocationPoolContext : BaseDbContext,IUnitOfWork
    {
        public LocationPoolContext()
        {
        }

        public LocationPoolContext(DbContextOptions<LocationPoolContext> options)
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