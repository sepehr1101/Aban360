using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Estate), Schema = TableSchema.Name)]
public class Estate: EstateBase
{
    public virtual ConstructionType ConstructionType { get; set; } = null!;

    public virtual EstateBoundType EstateBoundType { get; set; } = null!;

    public virtual ICollection<Flat> Flats { get; set; } = new List<Flat>();

    public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();

    public virtual ICollection<Estate> InversePrevious { get; set; } = new List<Estate>();

    public virtual Estate? Previous { get; set; }

    public virtual Usage UsageConsumtion { get; set; } = null!;//todo: UsageConsumption

    public virtual Usage UsageSell { get; set; } = null!;
    public virtual ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
}
