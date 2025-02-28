using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Contexts.Implementations
{
    public partial class CalculationPoolContext : BaseDbContext, IUnitOfWork
    {
        public CalculationPoolContext()
        {
        }

        public CalculationPoolContext(DbContextOptions<CalculationPoolContext> options)
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
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}