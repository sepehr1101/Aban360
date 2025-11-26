using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record CustomerInfoGetDto
    {
        public MembersInfo MembersInfo { get; set; }
        public LatestBedBesConsumptionInfo BedBesInfo { get; set; }
        public LatesTavizInfo TavizInfo { get; set; }
        public CustomerInfoGetDto(MembersInfo membersInfo, LatestBedBesConsumptionInfo bedBesInfo, LatesTavizInfo tavizInfo)
        {
            MembersInfo = membersInfo;
            BedBesInfo = bedBesInfo;
            TavizInfo = tavizInfo;
        }
    }
}
