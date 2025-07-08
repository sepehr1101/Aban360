namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record HouseholdRegisterUpdateDto
    {
        public string  BillId { get; set; }
        public int HouseholdNumber { get; set; }
        public string HouseholdDateJalali { get; set; }
    }
}
