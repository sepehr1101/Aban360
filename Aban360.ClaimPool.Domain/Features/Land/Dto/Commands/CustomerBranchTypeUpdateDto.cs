using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerBranchTypeUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int BranchTypeId { get; set; }
        public string ToDayDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public CustomerBranchTypeUpdateDto(int id, int zoneId, int customerNumber,string billId, int branchTypeId)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
            BranchTypeId = branchTypeId;
        }
        public CustomerBranchTypeUpdateDto()
        {
        }
    }
}
