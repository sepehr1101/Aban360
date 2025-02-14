namespace Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands
{
    public record SiphonTypeUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
