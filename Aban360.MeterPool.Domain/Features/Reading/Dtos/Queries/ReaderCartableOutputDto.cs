using Aban360.Common.BaseEntities;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record ReaderCartableOutputDto
    {
        public IEnumerable<TrackingDataOutputDto> TrackingInfo { get; set; }
        public IEnumerable<DetailDataOutputDto> DetailInfo { get; set; }
        public IEnumerable<NumericDictionary> Usages { get; set; }
        public IEnumerable<NumericDictionary> DeletionStates { get; set; }
        public IEnumerable<ReadingCounterStateGetDto> CounterStates { get; set; }
        public IEnumerable<NumericDictionary> BranchTypes { get; set; }
        public IEnumerable<NumericDictionary> Guilds { get; set; }
        public IEnumerable<StringDictionary> MapDictionary { get; set; }
        public JsonElement Traverse { get; set; }
    }
}
