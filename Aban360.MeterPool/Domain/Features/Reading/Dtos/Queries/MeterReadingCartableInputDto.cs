namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record MeterReadingCartableInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
    }
}
