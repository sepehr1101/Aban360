namespace Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands
{
    public record SiphonUpdateDto
    {
        public int Id { get; set; }
        public string? InstallationLocation { get; set; }
        public string? InstallationDate { get; set; }
        public short SiphonDiameterId { get; set; }
        public short SiphonTypeId { get; set; }
        public short SiphonMaterialId { get; set; }
        public Guid UserId { get; set; }
        public int? PreviousId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string InsertLogInfo { get; set; } = null!;
    }
}
