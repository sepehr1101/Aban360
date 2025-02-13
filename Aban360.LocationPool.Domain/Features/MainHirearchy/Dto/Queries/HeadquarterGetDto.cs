namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries
{
    public record HeadquarterGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ProvinceId { get; set; }
        public string ProvinceTitle { get; set; }
    }
}
