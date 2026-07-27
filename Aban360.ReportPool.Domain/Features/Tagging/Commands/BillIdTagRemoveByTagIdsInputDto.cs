namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record BillIdTagRemoveByTagIdsInputDto
    {
        public IEnumerable<int> TagIds { get; set; }
        public bool IsConfirm { get; set; }
    }
}
