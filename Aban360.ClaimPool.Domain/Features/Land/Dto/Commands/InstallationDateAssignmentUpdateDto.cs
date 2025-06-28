namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record InstallationDateAssignmentUpdateDto
    {
        public string BillId { get; set; }
        public string WaterInstallationDate { get; set; }
        public string SewageInstallationDate { get; set; }
    }
}
