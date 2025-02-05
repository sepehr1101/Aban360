namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries
{
    public record CountryGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
