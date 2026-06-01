using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerDeletionStateUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int DeletionStateId { get; set; }
        public string ToDayDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public CustomerDeletionStateUpdateDto(int id, int zoneId, int customerNumber, string billId, int deletionStateId)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
            DeletionStateId = deletionStateId;
        }
        public CustomerDeletionStateUpdateDto()
        {
        }
    }
}
