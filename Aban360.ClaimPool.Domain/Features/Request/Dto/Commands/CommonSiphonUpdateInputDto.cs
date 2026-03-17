namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record CommonSiphonUpdateInputDto
    {
        public int TrackNumber { get; set; }
        public string StringTrackNumber { get; set; }
        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
    }
}
