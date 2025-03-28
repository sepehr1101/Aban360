namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands
{
    public record UserWorkdayCreateDto
    {
        public Guid UserId { get; set; }
        public string UserFullname { get; set; } = null!;//todo: with user or backend?
        public string FromReadingNumber { get; set; } = null!;
        public string ToReadingNumber { get; set; } = null!;
        public string DateJalali { get; set; } = null!;//todo: with user or backend?
        public int ZoneId { get; set; }
    }
}
