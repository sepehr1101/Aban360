using Aban360.CommunicationPool.Domain.Constants;

namespace Aban360.CommunicationPool.Domain.Features.Sms.Commands
{
    public record SmsManagerInsertDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int? TrackNumber { get; set; }
        public string? BillId { get; set; }
        public string ReferenceId { get; set; }
        public int TypeId { get; set; }
        public string TypeTitle { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
        public DateTime? FetchDateTime { get; set; }
        public DateTime? SendDateTime { get; set; }
        public SmsManagerInsertDto(int? trackNumber, string? billId, int typeId)
        {
            TrackNumber = trackNumber;
            BillId = billId;
            TypeId = typeId;
            TypeTitle = SmsManagerType.Get(TypeId);
        }
    }
}
