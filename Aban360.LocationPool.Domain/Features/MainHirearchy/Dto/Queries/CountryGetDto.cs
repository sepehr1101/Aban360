namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries
{
    public record CountryGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
