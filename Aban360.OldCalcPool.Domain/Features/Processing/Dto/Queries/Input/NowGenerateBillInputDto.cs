namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record NowGenerateBillInputDto
    {
        public string BillId { get; set; }
        public int CurrentMeterNumber { get; set; }
        public string MobileNumber { get; set; }
    }
}
