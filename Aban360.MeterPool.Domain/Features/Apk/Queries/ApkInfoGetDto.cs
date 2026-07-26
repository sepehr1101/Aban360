using DNTPersianUtils.Core;

namespace Aban360.MeterPool.Domain.Features.Apk.Queries
{
    public record ApkInfoGetDto
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public byte[] FileContent { get; set; }
        public string? Description { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public string InsertedDateJalali { get { return InsertedDateTime.ToShortPersianDateTimeString(); } }

        public Guid? RemovedBy { get; set; }
        public DateTime? RemovedDateTime { get; set; }
        public string RemovedDateJalali { get { return RemovedDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }

        public Guid? ExpiredBy { get; set; }
        public DateTime? ExpiredDateTime { get; set; }
        public string ExpiredDateJalali { get { return ExpiredDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }

    }
}
