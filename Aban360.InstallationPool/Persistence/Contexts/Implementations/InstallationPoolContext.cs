using Aban360.Common.Db.Context;
using Aban360.InstallationPool.Persistence.Constants;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Contexts.Implementations
{
    public partial class InstallationPoolContext:BaseDbContext,IUnitOfWork
    {
        public InstallationPoolContext()
        {
        }

        public InstallationPoolContext(DbContextOptions<InstallationPoolContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(TableSchema.Name);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
