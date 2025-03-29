namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record OfficialHolidayGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string DateJalali { get; set; } = null!;
    }
}
