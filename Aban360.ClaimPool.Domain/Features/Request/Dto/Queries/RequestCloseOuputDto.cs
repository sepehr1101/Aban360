using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record RequestCloseOuputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int TrackNumber { get; set; }
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public RequestCloseOuputDto(int zoneId, string zoneTitle, int trackNumber)
        {
            ZoneId = zoneId;
            ZoneTitle = zoneTitle;
            TrackNumber = trackNumber;
        }
        public RequestCloseOuputDto()
        {
        }
    }
}
