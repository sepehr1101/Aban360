namespace Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries
{
    public record RepairDateValidateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int ReturnCauseId { get; set; }
    }
}