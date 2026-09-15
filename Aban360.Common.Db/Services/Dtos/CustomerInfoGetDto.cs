using Aban360.Common.BaseEntities;

namespace Aban360.Common.Db.Services.Dtos
{
    public record CustomerInfoGetDto
    {
        public MemberInfoGetDto MembersInfo { get; set; }
        public LatestBedBesConsumptionInfo BedBesInfo { get; set; }
        public LatesTavizInfo TavizInfo { get; set; }
        public CustomerInfoGetDto(MemberInfoGetDto membersInfo, LatestBedBesConsumptionInfo bedBesInfo, LatesTavizInfo tavizInfo)
        {
            MembersInfo = membersInfo;
            BedBesInfo = bedBesInfo;
            TavizInfo = tavizInfo;
        }
    }
    public record LatestBedBesConsumptionInfo
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string LastMeterDateJalali { get; set; }
        public int? LastMeterNumber { get; set; }
        public float? LastMonthlyConsumption { get; set; }
        public float? LastConsumption { get; set; }
        public int? LastCounterStateCode { get; set; }
        public string? LastCounterStateTitle { get; set; }
        public double? LastSumItems { get; set; }
        public bool IsReturned { get; set; }
    }
    public record LatesTavizInfo
    {
        public int CustomerNumber { get; set; }
        public string? TavizDateJalali { get; set; }
        public string? TavizCause { get; set; }
        public string? TavizRegisterDateJalali { get; set; }
        public int? TavizNumber { get; set; }
    }
}
