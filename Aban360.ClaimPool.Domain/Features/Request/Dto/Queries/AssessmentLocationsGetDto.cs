namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentLocationsGetDto
    {
        public string? XGis { get; set; }
        public string? YGis { get; set; }

        public string XMap { get; set; }
        public string YMap { get; set; }

        public string XGps { get; set; }
        public string YGps { get; set; }
        public string Accuracy { get; set; }
    }
}
