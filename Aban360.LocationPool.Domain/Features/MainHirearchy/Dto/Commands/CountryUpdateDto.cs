namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands
{
    public record CountryUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
