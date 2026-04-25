namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record RequestAfterSaleInputDto
    {
        public string BillId { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string NotificationNumber { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public bool HasSendSms { get; set; }
        public ICollection<int> SelectedServices { get; set; }
    }
}
