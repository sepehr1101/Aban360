namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Output
{
    public record AssessmentLogGetDto
    {
        public string FileName { get; set; }
        public string InsertDateTimeJalali { get; set; }
        public AssessmentLogGetDto(string fileName, string insertDateTimeJalali)
        {
            FileName = fileName;
            InsertDateTimeJalali = insertDateTimeJalali;
        }
        public AssessmentLogGetDto()
        {
        }
    }
}
