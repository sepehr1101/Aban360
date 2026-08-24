namespace Aban360.MeterPool.Domain.Features.Reading
{
    public record ReadingLoadDto
    {
        public int Id { get; set; }
        public string UsageTitle { get; set; } = null!;
        public int UsageId { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; } = null!;
        public int PreviousNumber { get; set; }
        public string PreviousDateJalalai { get; set; } = null!;
        public short PreviousCounterStateId { get; set; }
        public string PreviousCounterStateTitle { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Surename { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string BillId { get; set; } = null!;
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public int TrackNumber { get; set; }
        public long Debt { get; set; }
        public string PostalCode { get; set; } = null!;
        public string MainSiphonTitle { get; set; } = null!;
        public int MainSiphonId { get; set; }
        public string X { get; set; } = null!;
        public string Y { get; set; } = null!;
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int DiscountId { get; set; }
        public string DiscountTitle { get; set; } = null!;
        public int WaterDiameterId { get; set; }
        public string WaterDiameterTitle { get; set; } = null!;
        public string WaterInstallationDateJalali { get; set; } = null!;
        public string SewageInstallationDateJalali { get; set; } = null!;
        public int GuildId { get; set; }
        public string GuildTitle { get; set; } = null!;
        public int HouseholdUnit { get; set; }
        public string HouseholdDateJalali { get; set; } = null!;
    }
}
