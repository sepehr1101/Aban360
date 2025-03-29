using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Constants;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Contexts.Implementation
{
    public partial class ClaimPoolContext : BaseDbContext, IUnitOfWork
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