namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record CardexInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string? FromDate { get; set; }
        public CardexInputDto(int zoneId,int customerNumber,string? fromDate)
        {
            ZoneId=zoneId;
            CustomerNumber=customerNumber;
            FromDate=fromDate;
        }
    }
}
