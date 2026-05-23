using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record TankerHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string BillId { get; set; }
        public string PayId { get; set; }
        public string Title { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string RegisterDateJalali { get; set; }
        public string FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string Duration { get; set; }
        public int Consumption { get; set; }
        public long Amount { get; set; }
        public string? SaleState { get; set; }

        public long? PaymentAmount { get; set; }
        public string? PaymentId { get; set; }
        public string? BankCode { get; set; }
        public string? BankDateJalali { get; set; }
        public string? PaymentDateJalali { get; set; }
        public string? PaymentTypeTitle { get; set; }
        public int? PaymentTypeId { get; set; }
    }
}
