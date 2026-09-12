using DNTPersianUtils.Core;

namespace Aban360.CommunicationPool.Domain.Features.Sms.Queries
{
    public record SmsDraftGetDto
    {
        public Guid Id { get; set; }
        public int? TrackNumber { get; set; }
        public string? BillId { get; set; }
        public string ReferenceId { get; set; }
        public int TypeId { get; set; }
        public string TypeTitle { get; set; }
        public string Message { get; set; }
        public string MobileNumber { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string InsertDateJalali { get { return InsertDateTime.ToShortPersianDateTimeString(); } }
        public DateTime? FetchDateTime { get; set; }
        public string? FetchDateTJalali { get { return FetchDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }
        public DateTime? SendDateTime { get; set; }
        public string? SendDateTJalali { get { return SendDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }
    }
}
