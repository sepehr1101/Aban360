using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands
{
    public record DocumentCreateDto
    {
        public IFormFile document { get; set; }
        public string? Description { get; set; }
    }
    public record DocumentDeleteDto
    {
        public Guid Id { get; set; }
    }
    public record DocumentUpdateDto
    {
        public Guid Id { get; set; }
        public IFormFile document { get; set; }
        public string? Description { get; set; }
    }
    public record DocumentGetDto
    {
        public Guid Id { get; set; }
        public Guid FileRowId { get; set; }
        public string Name { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public long SizeInByte { get; set; }
        public string ContentType { get; set; } = null!;
        public byte[]? FileContent { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? Description { get; set; }
    }
}
