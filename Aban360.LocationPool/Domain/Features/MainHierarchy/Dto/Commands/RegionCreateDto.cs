namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands
{
    public record RegionCreateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public short HeadquartersId { get; set; }
    }
}
