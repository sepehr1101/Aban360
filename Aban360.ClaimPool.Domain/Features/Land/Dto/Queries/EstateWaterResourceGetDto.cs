namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record EstateWaterResourceGetDto
    {
        public short Id { get; set; }
        public int EstateId { get; set; }
        public string EstatePostalCode { get; set; }
        public short WaterResourceId { get; set; }
        public string WaterResourceTitle { get; set; }
    }
}
