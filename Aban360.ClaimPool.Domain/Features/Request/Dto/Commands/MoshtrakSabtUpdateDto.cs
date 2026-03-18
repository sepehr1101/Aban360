namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record MoshtrakSabtUpdateDto
    {
        public int TrackNumber { get; set; }
        public bool IsRegister { get; set; }
        public string? Description { get; set; }
        public MoshtrakSabtUpdateDto(int trackNumber, bool isRegister,string? description)
        {
            TrackNumber = trackNumber;
            IsRegister = isRegister;
            Description = description;
        }
        public MoshtrakSabtUpdateDto()
        {
        }
    }
}
