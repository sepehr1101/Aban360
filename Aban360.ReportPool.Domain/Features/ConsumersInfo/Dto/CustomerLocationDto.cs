namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record CustomerLocationDto
    {
        public string X { get; set; }
        public string Y { get; set; }
    }
    public record CustomerLocationInputDto
    {
        public string BuildId { get; set; }
        public CustomerLocationInputDto(string billId)
        {
            BuildId = billId;
        }
    }
}
