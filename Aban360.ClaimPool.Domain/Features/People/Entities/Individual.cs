using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(Individual))]
public partial class Individual
{
    public int Id { get; set; }
    public short IndividualTypeId { get; set; }
    public string FullName { get; set; } = null!;

    public string? NationalId { get; set; }

    public string? FatherName { get; set; }

    public string? PhoneNumbers { get; set; }

    public string? MobileNumbers { get; set; }

    public Guid UserId { get; set; }

    public int? PreviousId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual IndividualType IndividualType { get; set; } = null!;
    public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();

    public virtual ICollection<Individual> InversePrevious { get; set; } = new List<Individual>();

    public virtual Individual? Previous { get; set; }
}
