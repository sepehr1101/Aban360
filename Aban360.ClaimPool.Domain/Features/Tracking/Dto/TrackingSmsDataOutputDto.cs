namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingSmsDataOutputDto
    {
        public string Message { get; set; }
        public int DeliverySatateId { get; set; }
        public string DeliverySatateTitle { get; set; }
        public string InsertDateJalali { get; set; }
        public string InsertTime { get; set; }
    }
}
