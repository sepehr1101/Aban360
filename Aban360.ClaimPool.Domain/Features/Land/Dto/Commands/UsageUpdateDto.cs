namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ProvinceId { get; set; }
    }
}
