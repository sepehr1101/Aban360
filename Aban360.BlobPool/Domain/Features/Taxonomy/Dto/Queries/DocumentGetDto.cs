namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries
{
    public record DocumentGetDto
    {
        public Guid Id { get; set; }
        public short DocumentTypeId { get; set; }
        public Guid FileRowId { get; set; }
        public string Name { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public long SizeInByte { get; set; }
        public string ContentType { get; set; } = null!;
        public byte[]? FileContent { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? Description { get; set; }
        public bool IsThumbnail { get; set; } = false;
        public Guid? ParrentId { get; set; }
    }
}
