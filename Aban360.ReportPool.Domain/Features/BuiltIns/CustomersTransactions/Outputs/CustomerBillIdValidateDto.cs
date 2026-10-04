namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerBillIdValidateDto
    {
        public string BillId { get; set; }
        public bool IsValid { get; set; }
        public CustomerBillIdValidateDto(string billId,bool isValid)
        {
            BillId = billId;
            IsValid = isValid;
        }
        public CustomerBillIdValidateDto()
        {
        }
    }
}
