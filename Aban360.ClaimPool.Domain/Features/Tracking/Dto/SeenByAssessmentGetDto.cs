namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record SeenByAssessmentGetDto
    {
        public int TrackNumber { get; set; }
        public string RegisterDateGregorian { get; set; }
        public SeenByAssessmentGetDto(int trackNumber,string registerDateGregorian)
        {
            TrackNumber= trackNumber;
            RegisterDateGregorian= registerDateGregorian;
        }
        public SeenByAssessmentGetDto()
        {
        }
    }
}