namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands
{
    public record UserLeaveUpdateDto
    {
        public short Id { get; set; }
        public short RegiatereId { get; set; }
        public string RegiatereFullname { get; set; } = null!;
        public DateTime RegiatereDatetime { get; set; }
        public Guid UserId { get; set; }
        public string UserFullname { get; set; } = null!;
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
    }
}
