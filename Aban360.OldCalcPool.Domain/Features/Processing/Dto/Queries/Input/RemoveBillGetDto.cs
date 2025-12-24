namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record RemoveBillGetDto
    {
        public int Id{ get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public RemoveBillGetDto(int id,int zoneId,int customerNumber)
        {
            Id=id;
            ZoneId=zoneId;
            CustomerNumber=customerNumber;
        }
    }
}
