namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record MotherChildRequestInputDto
    {
        public int TrackNumber { get; set; }
        public int MotherCustomerNumber { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int Premises { get; set; }
        public int ImprovementCommercial { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementOverall { get; set; }
        public int ContractualCapacity { get; set; }
    }
}
