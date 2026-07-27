namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record TagsInputDto
    {
        public ICollection<int> TagIds { get; set; }
    }
}
