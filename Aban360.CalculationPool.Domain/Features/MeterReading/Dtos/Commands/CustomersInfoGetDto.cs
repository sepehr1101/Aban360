namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record CustomersInfoGetDto
    {
        public IEnumerable<MembersInfo> MembersInfo { get; set; }
        public IEnumerable<LatestBedBesConsumptionInfo> BedBesInfo { get; set; }
        public IEnumerable<LatesTavizInfo> TavizInfo { get; set; }
        public CustomersInfoGetDto(IEnumerable<MembersInfo> membersInfo, IEnumerable<LatestBedBesConsumptionInfo> bedBesInfo, IEnumerable<LatesTavizInfo> tavizInfo)
        {
            MembersInfo = membersInfo;
            BedBesInfo = bedBesInfo;
            TavizInfo = tavizInfo;
        }
    }
}
