namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record TrackingBillIdUpdateDto
    {
        public int TrackNumber { get; set; }
        public string BillId { get; set; }
        public TrackingBillIdUpdateDto(int trackNumber, string billId)
        {
            TrackNumber = trackNumber;
            BillId = billId;
        }
        public TrackingBillIdUpdateDto()
        {
        }
    }
}
