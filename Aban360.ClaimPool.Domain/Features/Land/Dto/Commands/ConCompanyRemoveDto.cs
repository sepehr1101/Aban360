namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ConCompanyRemoveDto
    {
        public int Id { get; set; }
        public Guid RemovedBy { get; set; }
        public DateTime RemovedDateTime { get; set; } = DateTime.Now;
        public ConCompanyRemoveDto(int id, Guid removedBy)
        {
            Id = id;
            RemovedBy = removedBy;
        }
        public ConCompanyRemoveDto()
        {
        }
    }
}
