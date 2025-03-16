using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Estate), Schema = TableSchema.Name)]
public class Estate
{
    public int Id { get; set; }

    public short ConstructionTypeId { get; set; }

    public short EstateBoundTypeId { get; set; }

    public string? PostalCode { get; set; }

    public string? X { get; set; }

    public string? Y { get; set; }

    public string? Parcel { get; set; }

    public string Address { get; set; } = null!;

    public int MunipulityId { get; set; }

    public short UsageSellId { get; set; }

    public short UsageConsumtionId { get; set; }

    public short UnitDomesticWater { get; set; }

    public short UnitCommercialWater { get; set; }

    public short UnitOtherWater { get; set; }

    public short UnitDomesticSewage { get; set; }

    public short UnitCommercialSewage { get; set; }

    public short UnitOtherSewage { get; set; }

    public short EmptyUnit { get; set; }

    public short HouseholdNumber { get; set; }

    public int Premises { get; set; }

    public int ImprovementsOverall { get; set; }

    public int ImprovementsDomestic { get; set; }

    public int ImprovementsCommercial { get; set; }

    public int ImprovementsOther { get; set; }

    public int ContractualCapacity { get; set; }

    public short Storeys { get; set; }

    public Guid UserId { get; set; }

    public int? PreviousId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual ConstructionType ConstructionType { get; set; } = null!;

    public virtual EstateBoundType EstateBoundType { get; set; } = null!;

    public virtual ICollection<Flat> Flats { get; set; } = new List<Flat>();

    public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();

    public virtual ICollection<Estate> InversePrevious { get; set; } = new List<Estate>();

    public virtual Estate? Previous { get; set; }

    public virtual Usage UsageConsumtion { get; set; } = null!;//todo: UsageConsumption

    public virtual Usage UsageSell { get; set; } = null!;
    public virtual ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
    public virtual ICollection<EstateWaterResource> EstateWaterResources { get; set; } = new List<EstateWaterResource>();

}
