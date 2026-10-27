namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillManualItemInputDto
    {
        public int ItemId { get; set; }
        public long Amount { get; set; }
    }
}
