using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Contexts.Implementations
{
    public partial class PaymentPoolContext
    {
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<PaymentMethod> PaymentProcedures { get; set; }
        public virtual DbSet<BankFileStructure> BankFileStructures { get; set; }
        public virtual DbSet<CreditorType> CreditorTypes{ get; set; }
        public virtual DbSet<AccountType> AccountTypes{ get; set; }
        public virtual DbSet<BankAccount> BankAccounts{ get; set; }
    }
}
