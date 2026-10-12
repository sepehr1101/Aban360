namespace Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands
{
    public record HRepairByRepairIdInsertDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int MinutesNumber { get; set; }
        public HRepairByRepairIdInsertDto(int id, int zoneId, int customerNumber, int minutesNumber)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            MinutesNumber = minutesNumber;
        }
        public HRepairByRepairIdInsertDto()
        {
        }
    }
}
