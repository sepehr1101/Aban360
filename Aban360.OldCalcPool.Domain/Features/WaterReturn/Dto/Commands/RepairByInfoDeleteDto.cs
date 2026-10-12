namespace Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands
{
    public record RepairByInfoDeleteDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public RepairByInfoDeleteDto(int id, int zoneId, int customerNumber)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
        }
        public RepairByInfoDeleteDto()
        {
        }
    }
}
