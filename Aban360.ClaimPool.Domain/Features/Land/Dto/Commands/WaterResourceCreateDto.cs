namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record WaterResourceCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public short HeadquartersId { get; set; }
    }
}
