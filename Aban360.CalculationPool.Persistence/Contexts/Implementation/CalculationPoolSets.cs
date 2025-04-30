using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Contexts.Implementations
{
    public partial class CalculationPoolContext
    {
        public virtual DbSet<Offering> Offerings { get; set; }

        public virtual DbSet<OfferingGroup> OfferingGroups { get; set; }

        public virtual DbSet<OfferingUnit> OfferingUnits { get; set; }
        public virtual DbSet<CompanyService> CompanyServices{ get; set; }
        public virtual DbSet<CompanyServiceType> CompanyServiceTypes{ get; set; }
        public virtual DbSet<CompanyServiceOffering> CompanyServiceOfferings{ get; set; }

        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
        public virtual DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
        public virtual DbSet<InvoiceLineItemInsertMode> InvoiceLineItemInsertModes { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceInstallment> InvoiceInstallments { get; set; }
        public virtual DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
        public virtual DbSet<WaterMeterChangeNumberHistory> WaterMeterChangeNumberHistories { get; set; }
        public virtual DbSet<LineItemType> LineItemTypes{ get; set; }
        public virtual DbSet<ImpactSign> ImpactSigns{ get; set; }
        public virtual DbSet<LineItemTypeGroup> LineItemTypeGroups{ get; set; }
        public virtual DbSet<TariffCalculationMode> TariffCalculationModes{ get; set; }
        public virtual DbSet<Tariff> Tariffs{ get; set; }
        public virtual DbSet<TariffByDetail> TariffByDetails{ get; set; }
        public virtual DbSet<TariffConstant> TariffConstants{ get; set; }
        public virtual DbSet<SupportedOperator> SupportedOperators { get; set; }

        public virtual DbSet<SupportedField> SupportedFields { get; set; }
    }
}
