namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerMobileUpdateInputDto
    {
        public string BillId { get; set; }
        public string MobileNumber { get; set; }
    }
}
