using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(CompanyService))]
public class CompanyService
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public bool IsMultiSelect { get; set; } = false;

    public short CompanyServiceTypeId { get; set; }

    public virtual ICollection<CompanyServiceOffering> CompanyServiceOfferings { get; set; } = new List<CompanyServiceOffering>();

    public virtual CompanyServiceType CompanyServiceType { get; set; } = null!;
}
