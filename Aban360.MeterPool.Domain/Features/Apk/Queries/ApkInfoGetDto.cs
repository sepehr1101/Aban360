using DNTPersianUtils.Core;

namespace Aban360.MeterPool.Domain.Features.Apk.Queries
{
    public record ApkInfoGetDto
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public byte[] FileContent { get; set; }
        public string? Description { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public string InsertedDateJalali { get { return InsertedDateTime.ToShortPersianDateTimeString(); } }
    }
    public record ApkInfo
    {       
        public short Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public byte[] FileContent { get; set; }
        public string? Description { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public string InsertedDateJalali { get { return InsertedDateTime.ToShortPersianDateTimeString(); } }
        public Guid? RemovedBy { get; set; }
        public Guid? ExpiredBy { get; set; }
    }
}
