using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record QueueInsertDto
    {
        public int OrderId { get; set; }
        public DateTime InsertDateTime { get { return DateTime.Now; } }
        public string InsertDateJalali { get { return InsertDateTime.ToShortPersianDateString(); } }
        public string InsertTime { get { return @$"{InsertDateTime.Hour}:{InsertDateTime.Minute}:{InsertDateTime.Second}"; } }
        public bool IsFinishedState { get; set; }
        public string Text { get; set; }
        public bool IsBatch { get; set; }
        public int FinalDeliveryState { get; set; }
        public int Retry { get; set; }
        public bool IsInProcess { get; set; }
        public string Receiver { get; set; }
        public string MagfaResult { get; set; }
        public int DeliveryRequestCount { get; set; }
        public int? Ticks { get; set; }
        public Guid QueueBatchWrapperId { get; set; }
        public Guid TrackingId { get; set; }
    }
}