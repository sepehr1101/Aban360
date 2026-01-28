namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record CustomerGeneralInfoGetDto
    {
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string UsageTitle { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }

    }
}
