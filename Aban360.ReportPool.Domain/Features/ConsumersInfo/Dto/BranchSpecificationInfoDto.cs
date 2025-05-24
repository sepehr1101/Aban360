namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BranchSpecificationInfoDto
    {
        //Water
        public string? ReadingNumber { get; set; }
        public int SealNumber { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string MeterProducerTitle { get; set; }
        public string MeterTypeTitle { get; set; }
        public string MeterUseTypeTitle { get; set; }
        public string WaterMeterInstallationMethodTitle{ get; set; }


        //Sewage
        public short  SiphonCount { get; set; }
        public short SiphonMaterialId { get; set; }


        //SiphonCount
        public Dictionary<string, short> SiphonsCount { get; set; } = new Dictionary<string, short>();
    }
}
