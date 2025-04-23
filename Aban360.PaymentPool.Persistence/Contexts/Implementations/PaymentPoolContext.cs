using Aban360.Common.Db.Context;
using Aban360.PaymentPool.Persistence.Constants;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Contexts.Implementations
{
    public partial class PaymentPoolContext: BaseDbContext, IUnitOfWork
    {
        public PaymentPoolContext()
        {
        }

        public PaymentPoolContext(DbContextOptions<PaymentPoolContext> options)
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
