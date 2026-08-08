namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsLoginInputDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
