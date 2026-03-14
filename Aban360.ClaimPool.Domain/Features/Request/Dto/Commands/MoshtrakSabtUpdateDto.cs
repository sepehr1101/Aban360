namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record MoshtrakSabtUpdateDto
    {
        public int TrackNumber { get; set; }
        public bool IsRegister { get; set; }
        public MoshtrakSabtUpdateDto(int trackNumber, bool isRegister)
        {
            TrackNumber = trackNumber;
            IsRegister = isRegister;
        }
        public MoshtrakSabtUpdateDto()
        {
        }
    }
}
