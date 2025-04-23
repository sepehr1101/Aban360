using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Contexts.Implementations
{
    public partial class PaymentPoolContext
    {
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<PaymentMethod> PaymentProcedures { get; set; }
    }
}
