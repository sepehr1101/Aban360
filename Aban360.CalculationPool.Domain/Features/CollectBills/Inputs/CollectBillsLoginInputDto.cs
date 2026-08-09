namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsLoginInputDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public CollectBillsLoginInputDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public CollectBillsLoginInputDto()
        {
        }
    }
}
