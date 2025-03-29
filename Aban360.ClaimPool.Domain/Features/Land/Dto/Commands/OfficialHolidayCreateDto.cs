namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record OfficialHolidayCreateDto
    {
        public string Title { get; set; } = null!;
        public string DateJalali { get; set; } = null!;//Todo : Write with User or Backend?
    }
}
