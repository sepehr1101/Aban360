namespace Aban360.MeterPool.Domain.Features.Apk.Commands
{
    public record ApkInfoRemoveDto
    {
        public short Id { get; set; }
        public Guid RemovedBy { get; set; }
        public DateTime RemovedDateTime { get; set; } = DateTime.Now;
        public ApkInfoRemoveDto(short id, Guid removedBy)
        {
            Id = id;
            RemovedBy = removedBy;
        }
        public ApkInfoRemoveDto()
        {
        }
    }
}
