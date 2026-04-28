using DNTPersianUtils.Core;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record AssessmentOffInsertDto
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public int AssessmentCode { get; set; }
        public Guid  AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string OffDateJalali { get; set; }
        public int InsertedByUserCode { get; set; }
        public string InsertedByUserName { get; set; }
        public DateTime InsertDateGregorian { get; set; }
        public string InsertDateJalali { get; set; }
        public string InsertTime { get; set; }

    }
}
