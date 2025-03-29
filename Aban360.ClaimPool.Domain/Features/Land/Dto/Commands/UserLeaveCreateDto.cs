namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UserLeaveCreateDto
    {
        public short RegiatereId { get; set; }
        public string RegiatereFullname { get; set; } = null!;
        public DateTime RegiatereDatetime { get; set; }
        public Guid UserId { get; set; }
        public string UserFullname { get; set; } = null!;
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
    }
}
