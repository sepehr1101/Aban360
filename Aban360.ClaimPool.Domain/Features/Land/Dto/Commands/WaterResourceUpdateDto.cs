namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record WaterResourceUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public short HeadquartersId { get; set; }
    }
}
