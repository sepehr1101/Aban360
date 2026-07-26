namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Output
{
    public record MeterApkLogGetDto
    {
        public string FileName { get; set; }
        public string InsertDateTimeJalali { get; set; }
        public MeterApkLogGetDto(string fileName, string insertDateTimeJalali)
        {
            FileName = fileName;
            InsertDateTimeJalali = insertDateTimeJalali;
        }
        public MeterApkLogGetDto()
        {
        }
    }
}
