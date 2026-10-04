namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record NewTrackingDuplicateValidationInputDto
    {
        public string? NeighbourBillId { get; set; }
        public string NationalCode { get; set; }
        public NewTrackingDuplicateValidationInputDto(string? neighbourBillId, string natianalCode)
        {
            NeighbourBillId = neighbourBillId;
            NationalCode = natianalCode;
        }
        public NewTrackingDuplicateValidationInputDto()
        {
        }
    }
}
