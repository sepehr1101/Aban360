namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record AssessmentResultInputDto
    {
        public Guid TrackingId { get; set; }
        public int TrackNumber { get; set; }
        public int ServiceGroupId { get; set; }//RequestType
        public string StringTrackNumber { get; set; }//ParNumber
        public string BillId { get; set; }
        public int CustomerNumber { get; set; }
        public string NeighbourBillId { get; set; }
        public int ZoneId { get; set; }
        public string NotificationMobile { get; set; }
        public int UsageId { get; set; }
        public int MeterDiameterId { get; set; }
        public int BranchTypeId { get; set; }
        public int DiscountTypeId { get; set; }
        public int TrackingResultId { get; set; }

        public ICollection<int> SelectedServices { get; set; }

        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }

        public int Premises { get; set; }
        public int ImprovementOverall { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementCommertial { get; set; }
        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
        public int MainSiphon { get; set; }
        public int CommonSiphon { get; set; }

        public int ContractualCapacity { get; set; }
        public int HouseValue { get; set; }
        public int CommertialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int DiscountCount { get; set; }
        public string NationalCode { get; set; }
        public string FatherName { get; set; }
        public string PostalCode { get; set; }
        public bool IsNonPermanent { get; set; }
        //public bool AdamTaxfifAb { get; set; }
        //public bool AdamTaxfifFazelab { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        //public int ResultId { get; set; }
        //
        public string X1 { get; set; }
        public string Y1 { get; set; }
        public string X2 { get; set; }
        public string Y2 { get; set; }
        public string Accuracy { get; set; }
        //
        public int? TrenchLenW { get; set; }
        public int? TrenchLenS { get; set; }
        public int? AsphaltLenW { get; set; }
        public int? AsphaltLenS { get; set; }
        public int? RockyLenW { get; set; }
        public int? RockyLenS { get; set; }
        public int? OtherLenW { get; set; }
        public int? OtherLenS { get; set; }
        public int? BasementDepth { get; set; }
        public bool? HasMap { get; set; }
        public string ReadingNumber { get; set; }
        //
        public string PreViewId { get; set; }
        public int CounterType { get; set; }
        public int InstallAgentState { get; set; }
        public string BlockId { get; set; }


        public int RegisterPlaqueNumber { get; set; }
        public int MeterMaterialId { get; set; }
        public bool HasWater { get; set; }
        public bool HasSewage { get; set; }
        public bool IsSpecial { get; set; }
        public string Map { get; set; }
        public string AssessmentSignature { get; set; }
        public string CustomerSignature { get; set; }
        public string CertificateNumber { get; set; }
        public string? LicenseIssuanceDateJalali { get; set; }
        public string? LicenseNum { get; set; }
    }
}
