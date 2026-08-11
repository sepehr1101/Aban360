namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsLoginOutputDto
    {
        public string token_access { get; set; }
        public DateTime in_expires { get; set; }//Type?
    }
}