using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.Request.Outputs
{
    public record ReceivedSmsHeaderOutputDto
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int RecordCount { get; set; }
        public int PageNumber { get; set; }
        public string ReprotDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public ReceivedSmsHeaderOutputDto(int recordCount, int pageNumber, string title, int totalRecords, int totalPages)
        {
            RecordCount = recordCount;
            PageNumber= pageNumber;
            Title = title;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
        }
        public ReceivedSmsHeaderOutputDto()
        {
        }
    }
}
