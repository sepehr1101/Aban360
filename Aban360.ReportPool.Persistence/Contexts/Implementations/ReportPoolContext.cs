using Aban360.Common.Db.Context;
using Aban360.ReportPool.Persistence.Contexts.Contracts;
using Aban360.ReportPool.Persistence.Contstants;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ReportPool.Persistence.Contexts.Implementations
{
    public partial class ReportPoolContext : BaseDbContext, IUnitOfWork
    {
        public ReportPoolContext()
        {
        }

        public ReportPoolContext(DbContextOptions<ReportPoolContext> options)
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
