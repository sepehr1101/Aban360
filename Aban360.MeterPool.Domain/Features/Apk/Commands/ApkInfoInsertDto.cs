using Microsoft.AspNetCore.Http;

namespace Aban360.MeterPool.Domain.Features.Apk.Commands
{
    public record ApkInfoInsertDto
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public byte[] FileContent { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedDateTime { get; set; } = DateTime.Now;
    }
}
