namespace Aban360.Common.BaseEntities
{
    public record ZoneIdAndReadingNumber
    {
        public int ZoneId { get; set; }
        public string ReadingNumber { get; set; }
        public ZoneIdAndReadingNumber(int zoneId, string readingNumber)
        {
            ZoneId = zoneId;
            ReadingNumber = readingNumber;
        }
        public ZoneIdAndReadingNumber()
        {
        }
    }
}
