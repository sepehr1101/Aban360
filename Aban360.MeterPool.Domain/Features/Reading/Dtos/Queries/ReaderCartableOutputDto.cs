using Aban360.Common.BaseEntities;

namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record ReaderCartableOutputDto
    {
        public IEnumerable<TrackingDataOutputDto> TrackingInfo { get; set; }
        public IEnumerable<DetailDataOutputDto> DetailInfo { get; set; }
        public IEnumerable<NumericDictionary> Usages { get; set; }
        public IEnumerable<NumericDictionary> DeletionStates { get; set; }
        public IEnumerable<NumericDictionary> CounterStates { get; set; }
        public IEnumerable<NumericDictionary> BranchTypes { get; set; }
    }
}
