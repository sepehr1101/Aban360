using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record SubscriptionAssignmentUpdateDto
    {
        public int Id { get; set; }
        public string BillId { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string ReadingNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

    }
}
