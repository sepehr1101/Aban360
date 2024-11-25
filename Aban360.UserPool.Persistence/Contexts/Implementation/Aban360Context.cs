using Aban360.UserPool.Persistence.Extensions;
using Aban360.UserPool.Persistence.Interceptors.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Contexts.Implementation
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
                var connectionString = MigrationRunner.GetConnectionInfo().Item1;
                optionsBuilder.UseSqlServer(connectionString);
                optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
