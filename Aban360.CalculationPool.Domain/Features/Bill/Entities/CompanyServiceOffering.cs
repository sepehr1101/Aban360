using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(CompanyServiceOffering))]
public class CompanyServiceOffering
{
    public short Id { get; set; }

    public short CompanyServiceId { get; set; }

    public short OfferingId { get; set; }

    public virtual CompanyService CompanyService { get; set; } = null!;

    public virtual Offering Offering { get; set; } = null!;
}
