namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record CustomerLocationWithUtmDto
    {
        public string? X { get; set; }
        public string? Y { get; set; }
        public double Easting { get; set; }
        public double Northing { get; set; }
        public int UtmZone { get; set; }
        public string? Letter { get; set; }
        public CustomerLocationWithUtmDto(string x, string y, double e, double n, int utmZone, string letter)
        {
            X = x;
            Y = y;
            Easting = e;
            Northing = n;
            UtmZone = utmZone;
            Letter = letter;
        }
    }
    public record CustomerLocationDto
    {
        public string? X { get; set; }
        public string? Y { get; set; }
    }
    public record CustomerLocationInputDto
    {
        public string BuildId { get; set; }
        public CustomerLocationInputDto(string billId)
        {
            BuildId = billId;
        }
    }
}
