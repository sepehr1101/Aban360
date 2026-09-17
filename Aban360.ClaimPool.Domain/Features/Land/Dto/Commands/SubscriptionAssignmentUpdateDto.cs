using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record SubscriptionAssignmentUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string ToDayDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public SubscriptionAssignmentUpdateDto(int id, int zoneId,int customerNumber, string billId, string readingNumber, string address, string postalCode)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
            Address = address;
            PostalCode = postalCode;
            ReadingNumber = readingNumber;
        }
        public SubscriptionAssignmentUpdateDto()
        {
        }
    }
}
