namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentGetDto
    {
        public int Code { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}