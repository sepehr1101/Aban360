namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record AssessmentOffInsertInputDto
    {
        public int AssessmentCode { get; set; }
        public string OffDateJalali { get; set; }

    }
}
