using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class ReadingConfigDefault
{
    public short Id { get; set; }

    public short NonReadDefault { get; set; }

    public short NonReadMax { get; set; }

    public short NonReadMin { get; set; }

    public bool PreNumberDisplayOption { get; set; }

    public bool BillIdDisplayOption { get; set; }

    public bool CustomerNumberDisplayOption { get; set; }

    public short DomesticLowConstBound { get; set; }

    public short DomesticLowPercentBound { get; set; }

    public short DomesticHighConstBound { get; set; }

    public short DomesticHighPercentBound { get; set; }

    public short ConstructionLowConstBound { get; set; }

    public short ConstructionLowPercentBound { get; set; }

    public short ConstructionHighConstBound { get; set; }

    public short ConstructionHighPercentBound { get; set; }

    public short ContractualCapacityLowConstBound { get; set; }

    public short ContractualCapacityLowPercentBound { get; set; }

    public short ContractualCapacityHighConstBound { get; set; }

    public short ContractualCapacityHighPercentBound { get; set; }

    public short NonDomesticLowPercentRateBound { get; set; }

    public short NonDomesticHighPercentRateBound { get; set; }

    public bool IsEnabled { get; set; }

    public bool PreDateDisplayOption { get; set; }

    public bool MobileDisplayOption { get; set; }

    public bool DebtDisplayOption { get; set; }

    public bool IconsDisplayOption { get; set; }

    public short HeadquartersId { get; set; }

    public string HeadquartersTitle { get; set; } = null!;
}
