namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record WaterResourceGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public short HeadquartersId { get; set; }
        public string HeadquartersTitle { get; set; } = null!;
    }
}
