namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record RequestBillDetailsRemoveDto
    {
        public int Id { get; set; }
        public string BillId { get; set; }
        public RequestBillDetailsRemoveDto(int id, string billId)
        {
            Id = id;
            BillId = billId;
        }
        public RequestBillDetailsRemoveDto()
        {
        }
    }
}
