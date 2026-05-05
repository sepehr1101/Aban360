namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillByBedBedIdInsertDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int TypeId { get; set; }
        public int BedBesId { get; set; }
        public BillByBedBedIdInsertDto(int zoneId,int customerNumber,int typeId,int bedBesId)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            TypeId = typeId;
            BedBesId = bedBesId;
        }
        public BillByBedBedIdInsertDto()
        {

        }
    }
}
