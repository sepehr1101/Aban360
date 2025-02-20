namespace Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries
{
    public record SiphonTypeGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
