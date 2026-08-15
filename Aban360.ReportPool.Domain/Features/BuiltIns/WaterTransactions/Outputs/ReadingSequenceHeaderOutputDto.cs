using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingSequenceHeaderOutputDto
    {
        public string  FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
