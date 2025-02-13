namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries
{
    public record ZoneGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public string UnstandardCode { get; set; }// = null!;
    }
}
