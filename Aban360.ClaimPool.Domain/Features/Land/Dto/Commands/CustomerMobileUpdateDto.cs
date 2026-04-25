using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerMobileUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string MobileNumber { get; set; }
        public string ToDayDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public CustomerMobileUpdateDto(int id,int zoneId, int customerNumber,string billId, string mobileNumber)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;    
            MobileNumber = mobileNumber;
        }
        public CustomerMobileUpdateDto()
        {
        }
    }
}
