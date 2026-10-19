using DNTPersianUtils.Core;

namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record TrackingDuplicateInsertDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public IEnumerable<Guid> TrackingIds { get; set; }
        public int FlowStepId { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
        public string InsertDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public Guid InsertBy { get; set; }

        public TrackingDuplicateInsertDto(IEnumerable<Guid> trackingIds, int flowStepId, Guid insertBy)
        {
            TrackingIds = trackingIds;
            FlowStepId = flowStepId;
            InsertBy = insertBy;
        }
        public TrackingDuplicateInsertDto()
        {
        }
    }
}
