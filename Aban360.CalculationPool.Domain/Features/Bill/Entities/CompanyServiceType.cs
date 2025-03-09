using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(CompanyServiceType))]
public class CompanyServiceType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public TariffCalculationModeEnum TariffCalculationModeId { get; set; } 

    public string? Description { get; set; }

    public virtual ICollection<CompanyService> CompanyServices { get; set; } = new List<CompanyService>();
    
    public virtual TariffCalculationMode TariffCalculationMode { get; set; }
}
