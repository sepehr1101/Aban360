namespace Aban360.Common.BaseEntities
{
    public record ZoneIdsAndReadingNumber
    {
        public IEnumerable<int> ZoneIds { get; set; }
        public string ReadingNumber { get; set; }
        public ZoneIdsAndReadingNumber(IEnumerable<int> zoneIds, string readingNumber)
        {
            ZoneIds = zoneIds;
            ReadingNumber = readingNumber;
        }
        public ZoneIdsAndReadingNumber()
        {
        }
    }
}
