using Aban360.UserPool.Persistence.Extensions;
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

        //public virtual DbSet<CaptchaDisplayMode> CaptchaDisplayMode { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = MigrationRunner.GetConnectionInfo().Item1;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(Provider).Assembly);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
