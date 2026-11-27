namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Output
{
    public record AssessmentLogFileGetDto
    {
        public string FileName { get; set; }
        public string? Content { get; set; }
        public string InsertDateTimeJalali { get; set; }
        public AssessmentLogFileGetDto(string fileName,string content, string insertDateTimeJalali)
        {
            FileName = fileName;
            Content = content;
            InsertDateTimeJalali = insertDateTimeJalali;
        }
        public AssessmentLogFileGetDto()
        {
        }
    }
}
