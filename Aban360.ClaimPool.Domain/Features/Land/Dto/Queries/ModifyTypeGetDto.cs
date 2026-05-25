namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ModifyTypeGetDto
    {
        public int RequestBillDetailsId { get; set; }
        public int Karten75Id { get; set; }
        public string Title { get; set; }
    }
}
