namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record MeterReplacementDateRegisterUpdateDto
    {
        public string  BillId { get; set; }
        public string ReplacementDateJalali { get; set; }
        public string MeterNumber { get; set; }
    }
}
