namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record ClientCommentInsertDto
    {
        public string BillId { get; set; }
        public string Comment { get; set; }
        public string UserDisplayName { get; set; }
        public Guid UserId { get; set; }
        public DateTime InsertDateTime { get; set; }=DateTime.Now;
    }
}
