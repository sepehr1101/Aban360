namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries
{
    public record MunicipalityGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int ZoneId { get; set; }
    }
}
