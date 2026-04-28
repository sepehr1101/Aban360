using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.Request.Outputs
{
    public record ReceivedSmsHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string ReprotDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title{ get; set; }
        public ReceivedSmsHeaderOutputDto(int recordCount,string title)
        {
            RecordCount = recordCount;
            Title = title;
        }
        public ReceivedSmsHeaderOutputDto()
        {
        }
    }
}
