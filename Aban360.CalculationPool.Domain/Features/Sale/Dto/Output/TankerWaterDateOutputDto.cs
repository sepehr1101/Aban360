namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record TankerWaterDateOutputDto
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string FirstName { get; set; }
        public string? Surname { get; set; }
        public string FullName { get; set; }
        public string? Address { get; set; }
        public int Barge { get; set; }
        public int Consumption { get; set; }
        public long Amount { get; set; }
        public string RegisterDateJalali { get; set; }
        public bool IsNotShorb { get; set; }

        public string? BillId { get; set; }
        public long? PaymentAmount { get; set; }
        public string? PaymentId { get; set; }
        public string? BankCode { get; set; }
        public string? BankDateJalali { get; set; }
        public string? PaymentDateJalali { get; set; }
        public string? PaymentTypeTitle { get; set; }
        public int? PaymentTypeId { get; set; }
    }
}
