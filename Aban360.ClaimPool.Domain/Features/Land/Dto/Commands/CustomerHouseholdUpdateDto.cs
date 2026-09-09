using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerHouseholdUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int HouseholdNumber { get; set; }
        public string HouseholdDateJalali { get; set; }
        public string ToDayDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public CustomerHouseholdUpdateDto(int id, int zoneId, int customerNumber, string billId, int householdNumber, string householdDateJalali)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
            HouseholdDateJalali = householdDateJalali;
            HouseholdNumber = householdNumber;
        }
        public CustomerHouseholdUpdateDto()
        {
        }
    }
}
