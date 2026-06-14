namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConnectDisconnectSetResultOutputDto
    {
        public string BillId { get; set; }
        public string MobileNumber { get; set; }
        public string Message { get; set; }
        public bool IsSend { get; set; }
        public ConnectDisconnectSetResultOutputDto(string billId, string mobileNumber, string message, bool isSend)
        {
            BillId = billId;
            MobileNumber = mobileNumber;
            Message = message;
            IsSend = isSend;
        }
        public ConnectDisconnectSetResultOutputDto()
        {
        }
    }
}
