using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record MeterInstallationUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string MeterInstallationDateJalali { get; set; }
        public string? SiphonInstallationDateJalali { get; set; }
        public string ToDayDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public MeterInstallationUpdateDto(int id, int zoneId, int customerNumber, string billId, string meterInstallationDateJalali, string? siphonInstallationDateJalali)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
            MeterInstallationDateJalali = meterInstallationDateJalali;
            SiphonInstallationDateJalali = siphonInstallationDateJalali ?? string.Empty;
        }
        public MeterInstallationUpdateDto()
        {
        }
    }
}
