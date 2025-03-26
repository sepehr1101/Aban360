namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands
{
    public record OfficialHolidayCreateDto
    {
        public string Title { get; set; } = null!;
        public string DateJalali { get; set; } = null!;//Todo : Write with User or Backend?
    }
}
