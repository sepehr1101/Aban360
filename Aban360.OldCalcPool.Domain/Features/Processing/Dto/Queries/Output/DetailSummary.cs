namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record DetailSummary<TSummary, TDetail>
    {
        public string Title{ get; set; }
        public IEnumerable<TDetail> Details { get; set; } = default!;
        public TSummary Summary { get; set; }= default!;

        public DetailSummary(string title,IEnumerable<TDetail> details,TSummary summary)
        {
            Title = title;
            Details = details;
            Summary = summary;
        }
    }
}
