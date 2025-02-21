namespace Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries
{
    public record SiphonMaterialGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
