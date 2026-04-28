namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record AssessmentOffGetDto
    {
        public Guid Id { get; set; }
        public int AssessmentCode { get; set; }
        public Guid AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string OffDateJalali { get; set; }
        public string InsertDateJalali { get; set; }
        public DateTime InsertDateGregorian { get; set; }
        public int InsertedByUserCode { get; set; }
        public string InsertedByUserName { get; set; }
        public string InsertedTime { get; set; }
        public bool IsCanceled { get; set; }
        public string CanceledDateJalali { get; set; }
        public DateTime CanceledDateGregorian { get; set; }
        public string CanceledTime { get; set; }
        public int CancellerCode { get; set; }
        public string CancellerName { get; set; }
    }
}
