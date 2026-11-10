namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record MoshtrakCommonSiphonUpdateDto
    {
        public int TrackNumber { get; set; }
        public bool HasCommonSiphon { get; set; }
        public MoshtrakCommonSiphonUpdateDto(int trackNumber, bool hasCommonSiphon)
        {
            TrackNumber = trackNumber;
            HasCommonSiphon = hasCommonSiphon;
        }
        public MoshtrakCommonSiphonUpdateDto()
        {
        }
    }
}
