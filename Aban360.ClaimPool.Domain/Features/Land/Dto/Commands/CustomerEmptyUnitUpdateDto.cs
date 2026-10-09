using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerEmptyUnitUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int EmptyUnit { get; set; }
        public string ToDayDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public CustomerEmptyUnitUpdateDto(int id, int zoneId, int customerNumber, string billId, int emptyUnit)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
            EmptyUnit = emptyUnit;
        }
        public CustomerEmptyUnitUpdateDto()
        {
        }
    }
}
