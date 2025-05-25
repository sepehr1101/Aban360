namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BranchSpecificationInfoDto
    {
        //Water
        public string MeterDiameterTitle { get; set; }
        public string? ReadingNumber { get; set; }
        public int SealNumber { get; set; }
        public string MeterTypeTitle { get; set; }
        public string MeterProducerTitle { get; set; }
        public string MeterEquipmentBrokerTitle{ get; set; }//
        public string InstallationBrokerTitle{ get; set; }//
        public string WaterMeterInstallationMethodTitle { get; set; }
        public short MeterLife{ get; set; }//
        public string MeterStatusTitle { get; set; }//
        public string WitnessMeter { get; set; }//


        //Sewage
        public string  CommonSiphon { get; set; }//
        public short  SiphonCount { get; set; }
        public short SiphonMaterialId { get; set; }
        public short SiphonLife { get; set; }//
        //3 Broker
        public short LoadOfContamination { get; set; }//


        //SiphonCount
        public IEnumerable<SiphonsDiameterCount> SiphonsDiameterCount { get; set; }
    }
    public record SiphonsDiameterCount
    {
        public string SiphonDiameterTitle { get; set; }
        public short SiphonCount { get; set; }
    }
}
