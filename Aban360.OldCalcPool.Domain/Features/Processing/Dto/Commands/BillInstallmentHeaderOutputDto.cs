namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillInstallmentHeaderOutputDto
    {
        public string FullName { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousMeter { get; set; }
        public int CurrentMeter { get; set; }
        public int Consumption { get; set; }
        public long Payable { get; set; }
        public string RegisterDateJalali { get; set; }
        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }
    }
}
