namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BillIdListDto
    {
        public string BillId { get; set; } = default!;
    }
    public record BillIdListDtoWrapper
    {
        public ICollection<BillIdListDto>? BillIdList { get; set; }
    }
}
