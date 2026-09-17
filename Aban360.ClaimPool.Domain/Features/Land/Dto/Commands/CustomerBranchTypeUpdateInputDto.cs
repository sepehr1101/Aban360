namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerBranchTypeUpdateInputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
    }
}
