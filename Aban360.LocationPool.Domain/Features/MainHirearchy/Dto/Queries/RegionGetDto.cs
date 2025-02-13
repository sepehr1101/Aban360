namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries
{
    public record RegionGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public short HeadquartersId { get; set; }
        public string HeadquartersTitle { get; set; }
    }
}
