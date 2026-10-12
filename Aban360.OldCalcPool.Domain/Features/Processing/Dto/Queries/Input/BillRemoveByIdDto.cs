namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record BillRemoveByIdDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public BillRemoveByIdDto(int id, int zoneId, int customerNumber)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
        }
        public BillRemoveByIdDto()
        {
        }
    }
}
