namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BranchSpecificationInfoDto
    {
        #region Water
        public string MeterDiameterTitle { get; set; }
        public string? BodySerial { get; set; }
        public int SealNumber { get; set; }

        public string MeterTypeTitle { get; set; }
        public string MeterProducerTitle { get; set; }
        public string MeterEquipmentBrokerTitle{ get; set; }//


        public string MeterInstallationBrokerTitle{ get; set; }//
        public string WaterMeterInstallationMethodTitle { get; set; }
        public short MeterLife{ get; set; }//

        public string MeterStatusTitle { get; set; }//
        public string WitnessMeter { get; set; }//
        public string WaterInstallDate { get; set; }
        #endregion

        #region Sewage
        public string  CommonSiphon { get; set; }//
        public short  SiphonCount { get; set; }
        public string SiphonMaterialTitle { get; set; }

        public short SiphonLife { get; set; }//
        public string SiphonInstallationContractor { get; set; }
        public string SiphonEquipmentBrokerTitle { get; set; }//

        public string SiphonInstallationBrokerTitle { get; set; }//
        public bool LoadOfContamination { get; set; }//
        public string SiphonInstallationDate { get; set; }

        #endregion

        //SiphonCount
        public IEnumerable<SiphonsDiameterCount> SiphonsDiameterCount { get; set; }
    }
    public record SiphonsDiameterCount
    {
        public string SiphonDiameterTitle { get; set; }
        public short SiphonCount { get; set; }
    }
}
