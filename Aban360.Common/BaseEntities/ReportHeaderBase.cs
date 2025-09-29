namespace Aban360.Common.BaseEntities
{
    public abstract record ReportHeaderBase
    {
        public string Title { get; set; } = default!;
        public int RowCount { get; set; }
        public string ReportDateJalali { get; set; } = default!;
    }
}
