namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillsUpdateOldDbDelDto
    {
        public int Id { get; set; }
        public int CustomerNumber { get; set; }
        public int ZoneId { get; set; }
        public bool Del { get; set; }
        public BillsUpdateOldDbDelDto(int id, int customerNumber, int zoneId, bool del)
        {
            Id = id;
            CustomerNumber = customerNumber;
            ZoneId = zoneId;
            Del = del;
        }
        public BillsUpdateOldDbDelDto()
        {
        }
    }
}
