using DNTPersianUtils.Core;

public record BillItemsHeaderOutputDto
{
    public int Id { get; set; }
    public int CustomerNumber { get; set; }
    public string BillId { get; set; }
    public int RegionId { get; set; }
    public string RegionTitle { get; set; }
    public int ZoneId { get; set; }
    public string ZoneTitle { get; set; }
    public int UsageId { get; set; }
    public string UsageTitle { get; set; }
    public int BranchTypeId { get; set; }
    public string BranchTypeTitle { get; set; }

    public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
    public string Title { get; set; }
}