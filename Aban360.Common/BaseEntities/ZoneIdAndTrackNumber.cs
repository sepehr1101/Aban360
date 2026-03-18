namespace Aban360.Common.BaseEntities
{
    public record ZoneIdAndTrackNumber
    {
        public int ZoneId { get; set; }
        public int TrackNumber { get; set; }
        public ZoneIdAndTrackNumber(int zoneId, int trackNumber)
        {
            ZoneId = zoneId;
            TrackNumber = trackNumber;
        }
        public ZoneIdAndTrackNumber()
        {
        }
    }
}
