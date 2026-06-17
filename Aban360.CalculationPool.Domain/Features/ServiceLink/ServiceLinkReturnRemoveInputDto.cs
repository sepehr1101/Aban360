namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkReturnRemoveInputDto
    {
        public string BillId { get; set; }
        public int Id { get; set; }//requestBillDetailId
    }
}
