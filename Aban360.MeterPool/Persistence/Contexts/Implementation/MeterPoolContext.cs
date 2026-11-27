using Aban360.Common.Db.Context;
using Aban360.MeterPool.Persistence.Constants;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Contexts.Implementations
{
    public partial class MeterPoolContext : BaseDbContext, IUnitOfWork
    {
        public MeterPoolContext()
        {
        }

        public MeterPoolContext(DbContextOptions<MeterPoolContext> options)
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
