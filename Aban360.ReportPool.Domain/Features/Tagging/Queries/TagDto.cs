using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int MainTagGroupId { get; set; }
        public string MainTagGroupTitle { get; set; } = string.Empty;
        public int TagGroupId { get; set; }
        public string TagGroupTitle { get; set; } = string.Empty;
        public string StringCode { get; set; }
        public DateTime? DeleteDateTime { get; set; }
        public string? DeleteDateJalali { get { return DeleteDateTime?.ToShortPersianDateTimeString(); } }
    }
}
