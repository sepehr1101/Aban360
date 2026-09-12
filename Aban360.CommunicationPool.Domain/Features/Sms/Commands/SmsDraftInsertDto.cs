using Aban360.CommunicationPool.Domain.Constants;

namespace Aban360.CommunicationPool.Domain.Features.Sms.Commands
{
    public record SmsDraftInsertDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int? TrackNumber { get; set; }
        public string? BillId { get; set; }
        public string ReferenceId { get; set; }
        public int TypeId { get; set; }
        public string TypeTitle { get; set; }
        public string Message { get; set; }
        public string MobileNumber { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
        public DateTime? FetchDateTime { get; set; }
        public DateTime? SendDateTime { get; set; }
        public SmsDraftInsertDto(int? trackNumber, string? billId, int typeId, string message, string mobileNumber,string referenceId)
        {
            TrackNumber = trackNumber;
            BillId = billId;
            TypeId = typeId;
            TypeTitle = SmsDraftType.GetTitle(TypeId);
            Message = message;
            MobileNumber = mobileNumber;
            ReferenceId = referenceId;
        }
    }
}
