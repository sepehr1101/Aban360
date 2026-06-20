namespace Aban360.ReportPool.Domain.Features.Dms
{
    public record ClientDiscountRemoveDto
    {
        public int Id { get; set; }
        public Guid RemovedBy { get; set; }
        public DateTime RemovedDateTime { get; set; } = DateTime.Now;
        public ClientDiscountRemoveDto(int id, Guid removedBy)
        {
            Id = id;
            RemovedBy = removedBy;
        }
        public ClientDiscountRemoveDto()
        {
        }
    }
}
