namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsIdentityInputDto
    {
        public string Token { get; set; }
        public CollectBillsIdentityInputDto(string token)
        {
            Token = token;
        }
        public CollectBillsIdentityInputDto()
        {
        }
    }
}
