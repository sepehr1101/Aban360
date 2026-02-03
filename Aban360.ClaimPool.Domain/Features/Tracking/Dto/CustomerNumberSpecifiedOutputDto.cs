namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record CustomerNumberSpecifiedOutputDto
    {
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
    }
}