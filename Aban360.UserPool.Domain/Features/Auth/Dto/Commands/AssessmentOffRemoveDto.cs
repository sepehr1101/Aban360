namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record AssessmentOffRemoveDto
    {
        public Guid Id { get; set; }
        public DateTime CancelDateTimeGregorian { get; set; }
        public string CancelTime { get; set; }
        public int CancellerCode { get; set; }
        public string CancellerName { get; set; }
    }
}
