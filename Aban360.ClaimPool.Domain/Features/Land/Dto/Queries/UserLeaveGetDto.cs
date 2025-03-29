namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UserLeaveGetDto
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
