namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BranchSpecificationInfoDto
    {
        public string  CustomerNumber { get; set; }
        public int ZoneId { get; set; }
        public string LatestMeterChangeDate { get; set; }
        #region Water
        public string MeterDiameterTitle { get; set; } = default!;
        public string? BodySerial { get; set; }
        public string? SealNumber { get; set; }

        public string? MeterTypeTitle { get; set; }
        public string? MeterProducerTitle { get; set; }
        public string? MeterEquipmentBrokerTitle{ get; set; }//


        public string? MeterInstallationBrokerTitle{ get; set; }//
        public string? WaterMeterInstallationMethodTitle { get; set; }
        public string MeterLife{ get; set; }//

        public string? MeterStatusTitle { get; set; }//
        public string? WitnessMeter { get; set; }//
        public string WaterInstallDate { get; set; } = default!;
        #endregion

        #region Sewage
        public bool  CommonSiphon { get; set; }//
        public short  SiphonCount { get; set; }
        public string? SiphonMaterialTitle { get; set; }

        public string SiphonLife { get; set; }//
        public string? SiphonInstallationContractor { get; set; }
        public string? SiphonEquipmentBrokerTitle { get; set; }//

        public string? SiphonInstallationBrokerTitle { get; set; }//
        public bool LoadOfContamination { get; set; }//
        public string SiphonInstallationDate { get; set; } = default!;

        public bool HasSewage { get; set; }
        public string? LastChangeSiphonDate { get; set; }

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
