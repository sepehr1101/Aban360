namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerHouseholdUpdateInputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }

        public string HouseholdDateJalali { get; set; }
        public int HouseholdNumber { get; set; }
    }
}
