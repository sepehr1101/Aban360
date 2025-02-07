namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries
{
    public record HeadquarterGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ProvinceId { get; set; }
    }
}
