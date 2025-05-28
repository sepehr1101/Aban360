namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BranchHistoryInfoDto
    {
        #region Water
        //watermeter
        public string WaterRequestDate { get; set; }//
        public string WaterInstallationDate { get; set; }
        public string WaterRegistrationDate { get; set; }//

        public string WaterReplacementDate { get; set; }//
        public string GuaranteeDate { get; set; }
        public string LastTemporaryDisconnectionDate { get; set; }//

        public string LastReconnectionDate { get; set; }//
        public string WaterSubscriptionCancellationDate { get; set; }//
        public string LastMeterReadingDate { get; set; }//

        public string LastPaymentDate { get; set; }//
        public string LattestChangeMianInfoDate { get; set; }//
        public string LastWaterBillRefundDate { get; set; }//

        public string LastSubscriptionRefundDate { get; set; }//
        public string HouseholdCountStartDate { get; set; }//
        public string HouseholdCountEndDate { get; set; }//




        //other
        #endregion

        #region Sewage
        public string? SewageRequestDate { get; set; }
        public string? SewageInstallationDate { get; set; }
        public string? SewageRegistrationDate { get; set; }

        public string? SiphonReplacementDate { get; set; }

        #endregion
    }
}
