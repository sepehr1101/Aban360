using Aban360.Common.Db.Context;
using Aban360.WorkflowPool.Persistence.Constants;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.WorkflowPool.Persistence.Contexts.Implementation
{
    public partial class WorkflowPoolContext : BaseDbContext, IUnitOfWork
    {
        public WorkflowPoolContext()
        {
        }

        public WorkflowPoolContext(DbContextOptions<WorkflowPoolContext> options)
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