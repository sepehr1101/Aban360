namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record NewTrackingDuplicateValidationInputDto
    {
        public string NeighbourBillId { get; set; }
        public string NationalCode { get; set; }
    }
}
