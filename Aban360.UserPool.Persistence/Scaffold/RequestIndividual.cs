using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class RequestIndividual
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

    public virtual ICollection<RequestIndividual> InversePrevious { get; set; } = new List<RequestIndividual>();

    public virtual RequestIndividual? Previous { get; set; }

    public virtual ICollection<RequestIndividualDiscountType> RequestIndividualDiscountTypes { get; set; } = new List<RequestIndividualDiscountType>();

    public virtual ICollection<RequestIndividualEstate> RequestIndividualEstateEstates { get; set; } = new List<RequestIndividualEstate>();

    public virtual ICollection<RequestIndividualEstate> RequestIndividualEstateIndividuals { get; set; } = new List<RequestIndividualEstate>();

    public virtual ICollection<RequestIndividualTag> RequestIndividualTags { get; set; } = new List<RequestIndividualTag>();
}
