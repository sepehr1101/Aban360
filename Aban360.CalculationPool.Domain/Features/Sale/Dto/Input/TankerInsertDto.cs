using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerInsertDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public int Barge { get; set; }
        public int Consumption { get; set; }
        public long Amount { get; set; }
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public bool IsNotShorb { get; set; }
        public string? ReadingNumber { get; set; }
    }
}
