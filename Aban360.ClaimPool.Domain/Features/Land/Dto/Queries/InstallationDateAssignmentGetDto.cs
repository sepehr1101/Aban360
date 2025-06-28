namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record InstallationDateAssignmentGetDto
    {
        public string WaterInstallationDate { get; set; }
        public string SewageInstallationDate { get; set; }
    }
}
