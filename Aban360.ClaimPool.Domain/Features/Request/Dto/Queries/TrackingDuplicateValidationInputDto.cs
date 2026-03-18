using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record TrackingDuplicateValidationInputDto
    {
        public string? BillId { get; set; }
        public string? NeighbourBillId { get; set; }
        public string? NationalCode { get; set; }
        public TrackingDuplicateValidationTypeEnum  ValidationType{ get; set; }
        public TrackingDuplicateValidationInputDto(string? billId,string? neigbourBillId,string? nationalCode, TrackingDuplicateValidationTypeEnum validationType)
        {
            BillId = billId;
            NeighbourBillId = neigbourBillId;
            NationalCode = nationalCode;
            ValidationType = validationType;
        }
        public TrackingDuplicateValidationInputDto()
        {
        }
    }
}
