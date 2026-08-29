using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public class TagGroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string StringCode { get; set; }
        public int MainTagGroupId { get; set; }
        public string MainTagGroupTitle { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CreateDateTimeJalali { get { return CreateDateTime.ToShortPersianDateTimeString(); } }
        public DateTime? DeleteDateTime { get; set; }
        public string DeleteDateTimeJalali { get { return DeleteDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }
    }
}
