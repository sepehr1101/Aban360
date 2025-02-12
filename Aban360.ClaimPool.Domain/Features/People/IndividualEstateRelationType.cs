﻿using System;
using System.Collections.Generic;

namespace Aban360.ClaimPool.Domain.Features.People;

public partial class IndividualEstateRelationType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();
}
