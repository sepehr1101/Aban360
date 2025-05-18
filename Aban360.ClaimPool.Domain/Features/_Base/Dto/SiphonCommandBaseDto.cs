namespace Aban360.ClaimPool.Domain.Features._Base.Dto
{
    public record SiphonCommandBaseDto
    {
        public string? InstallationLocation { get; set; }
        public string? InstallationDate { get; set; }
        public short SiphonDiameterId { get; set; }
        public short SiphonTypeId { get; set; }
        public short SiphonMaterialId { get; set; }
        public int WaterMeterId { get; set; }
    }
}
