namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record EstateWaterResourceCreateDto
    {
        public int EstateId { get; set; }
        public short WaterResourceId { get; set; }
    }
}
