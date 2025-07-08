namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record EmptyUnitRegisterUpdateDto
    {
        public string BillId { get; set; }
        public int EmptyUnit { get; set; }
    }
}
