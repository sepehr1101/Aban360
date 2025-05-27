namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ViolationInfoDto
    {
        public string WaterViolationType { get; set; }
        public string WastewaterViolationType { get; set; }
        public string PenaltyAmount { get; set; }
        public string OutstandingViolationBalance { get; set; }
        public string ViolationDurationDays { get; set; }
        public short IllegalWaterConsumptionVolume { get; set; }
        public short IllegalWastewaterDischargeVolume { get; set; }
        public string ViolationStartDate { get; set; }
    }
}
