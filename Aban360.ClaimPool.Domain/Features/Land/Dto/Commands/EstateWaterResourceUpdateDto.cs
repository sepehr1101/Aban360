namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record EstateWaterResourceUpdateDto
    {
        public short Id { get; set; }
        public int EstateId { get; set; }
        public short WaterResourceId { get; set; }
    }
}
