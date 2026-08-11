namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record SubscriptionAssignmentByTrackNumberUpdateDto
    {
        public int TrackNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

    }
}
