namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillInstallmentHeaderOutputDto
    {
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? NationalCode { get; set; }
        public string BillId { get; set; }
        public long Payable { get; set; }
        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }
    }
}
