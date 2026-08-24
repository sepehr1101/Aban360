using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillTransactionDetailWithLastReadingDataHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string FirstName { get; set; }
        public string? Surname { get; set; }
        public string FullName { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public string? LatestMeterChangeDateJalali { get; set; }

        public string? PreviousMeterDateJalali { get; set; }
        public int PreviousMeterNumber { get; set; }

        //billInfo
        public int Id { get; set; }
        public string CurrentMeterDeteJalali { get; set; }
        public int CurrentMeterNumber { get; set; }
        public int CurrentCounterStateCode { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public string ReadingNumber { get; set; }
        public double Amount { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public double? Consumption { get; set; }
        public double? ConsumptionAverage { get; set; }
        public bool HasAttentionCounterState { get; set; }


    }
}
