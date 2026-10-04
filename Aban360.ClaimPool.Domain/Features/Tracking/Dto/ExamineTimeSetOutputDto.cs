using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record ExamineTimeSetOutputDto
    {
        public string BillId { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }

        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentMobile { get; set; }
        public string AssessmentDayJalali { get; set; }
        public string FullName { get; set; }
        public int TrackNumber { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public IEnumerable<NumericDictionary> ServiceSelected { get; set; }
    }
}