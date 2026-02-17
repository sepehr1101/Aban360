using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentTasksOutputDto
    {
        public IEnumerable<AssessmentLocationInfoOutputDto> LocationsInfo { get; set; }
        public IEnumerable<NumericDictionary> Usages { get; set; }
        public IEnumerable<NumericDictionary> BranchTypes { get; set; }
        public IEnumerable<NumericDictionary> TrackingResults { get; set; }
        public IEnumerable<NumericDictionary> MeterDiameters { get; set; }
        public IEnumerable<NumericDictionary> SiphonDiameters { get; set; }
        public IEnumerable<NumericDictionary> DiscountTypes { get; set; }
        public IEnumerable<NumericDictionary>  MeterMaterials{ get; set; }
        public IEnumerable<NumericDictionary>  ArchiveFileTypes{ get; set; }
        public IEnumerable<StringDictionary> BlockCodes{ get; set; }
    }
}