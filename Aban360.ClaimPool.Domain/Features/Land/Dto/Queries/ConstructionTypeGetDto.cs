namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConstructionTypeGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
