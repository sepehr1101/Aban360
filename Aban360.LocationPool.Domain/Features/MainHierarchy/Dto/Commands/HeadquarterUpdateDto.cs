namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands
{
    public record HeadquarterUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ProvinceId { get; set; }
    }
}
