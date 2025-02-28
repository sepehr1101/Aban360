using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Contexts.Implementations
{
    public partial class CalculationPoolContext
    {
        public virtual DbSet<Offering> Offerings { get; set; }

        public virtual DbSet<OfferingGroup> OfferingGroups { get; set; }

        public virtual DbSet<OfferingUnit> OfferingUnits { get; set; }

        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
        public virtual DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
        public virtual DbSet<InvoinceLineItemInsertMode> InvoinceLineItemInsertModes { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceInstallment> InvoiceInstallments { get; set; }
        public virtual DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
    }
}
