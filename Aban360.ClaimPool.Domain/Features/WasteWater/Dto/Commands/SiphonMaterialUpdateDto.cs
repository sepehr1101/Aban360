namespace Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands
{
    public record SiphonMaterialUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
