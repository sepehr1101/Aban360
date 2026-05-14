using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record UnconfirmedBillReturnHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public long Amount { get; set; }
    }
}
