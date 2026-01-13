namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillConfirmeByBillIdInputDto
    {
        public string BillId { get; set; }
        public int JalaseNumber { get; set; }
    }
}
