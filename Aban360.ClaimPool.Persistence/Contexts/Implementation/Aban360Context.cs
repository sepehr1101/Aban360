using Aban360.ClaimPool.Persistence.Contexts.Implementation;
using Aban360.UserPool.Persistence.Extensions;
using Aban360.UserPool.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aban360.ClaimPool.Persistence.Contexts.Implementation
{
    public partial class Aban360Context : BaseDbContext
    {
        public Aban360Context()
        {
        }

        public Aban360Context(DbContextOptions<Aban360Context> options)
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