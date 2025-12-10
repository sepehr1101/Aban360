namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record KeyValueDto
    {
        public string Title { get; }
        public float Count { get; }
        public KeyValueDto(string title, float count)
        {
            Title = title;
            Count = count;
        }
    }
}
