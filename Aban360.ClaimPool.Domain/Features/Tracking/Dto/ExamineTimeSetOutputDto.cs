namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record ExamineTimeSetOutputDto
    {
        public int ExaminerCode { get; set; }
        public string ExaminerName { get; set; }
        public string ExaminerMobile { get; set; }
        public string ExaminerDayJalali { get; set; }
        public string FullName { get; set; }
        public int TrackNumber { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }

    }
}