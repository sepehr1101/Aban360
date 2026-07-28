using Aban360.ClaimPool.Domain.Constants;
using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record MeterReadingExcelFileDownloadHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }


        public int CommonCounterState { get { return (int)CounterStateCodeEnum.Common; } }
        public int MalfunctionCounterState { get { return (int)CounterStateCodeEnum.Malfunction; } }
        public int ChangeCounterState { get { return (int)CounterStateCodeEnum.Change; } }
        public int ReverseCounterState { get { return (int)CounterStateCodeEnum.Reverse; } }
        public int CloseCounterState { get { return (int)CounterStateCodeEnum.Close; } }
        public int NextRoundCounterState { get { return (int)CounterStateCodeEnum.NextRound; } }
        public int WithoutConsumptionCounterState { get { return (int)CounterStateCodeEnum.WithoutConsumption; } }
        public int BlockCounterState { get { return (int)CounterStateCodeEnum.Block; } }
        public int NonReadCounterState { get { return (int)CounterStateCodeEnum.NonRead; } }
        public int DesolateUnitCounterState { get { return (int)CounterStateCodeEnum.DesolateUnit; } }
        public int DisconnectionCounterState { get { return (int)CounterStateCodeEnum.Disconnection; } }

    }
}
