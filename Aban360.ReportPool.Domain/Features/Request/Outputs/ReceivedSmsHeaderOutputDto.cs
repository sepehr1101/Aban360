using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.Request.Outputs
{
    public record ReceivedSmsHeaderOutputDto
    {
        public int TotalRecords { get; set; }
        public string ReprotDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public ReceivedSmsHeaderOutputDto(string title, int totalRecords)
        {
            Title = title;
            TotalRecords = totalRecords;
        }
        public ReceivedSmsHeaderOutputDto()
        {
        }
    }
}
