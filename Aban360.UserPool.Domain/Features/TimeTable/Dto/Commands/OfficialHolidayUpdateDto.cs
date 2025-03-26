namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands
{
    public record OfficialHolidayUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string DateJalali { get; set; } = null!;
    }
}
