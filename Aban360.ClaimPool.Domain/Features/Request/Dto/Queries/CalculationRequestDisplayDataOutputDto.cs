namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record CalculationRequestDisplayDataOutputDto
    {
        public string Title { get; set; }
        public long Amount { get; set; }
        public long Discount { get; set; }
    }
}   
