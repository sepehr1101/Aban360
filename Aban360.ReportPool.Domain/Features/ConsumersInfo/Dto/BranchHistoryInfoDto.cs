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
        public string? GuaranteeDate { get; set; }
        public string LattestBranchTemporaryOffDate { get; set; }//

        public string LattestBranchTemporaryOnDate { get; set; }//
        public string BranchCollectedDate { get; set; }//
        public string LattestReadingDate { get; set; }//

        public string LattestPayDate { get; set; }//
        public string LattestChangeMianInfoDate { get; set; }//


        //other
        #endregion

        #region Sewage
        public string? SewageInstallationDate { get; set; }

        #endregion
    }
}
