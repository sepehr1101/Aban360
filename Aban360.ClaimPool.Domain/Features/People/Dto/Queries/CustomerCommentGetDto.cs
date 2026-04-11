namespace Aban360.ClaimPool.Domain.Features.People.Dto.Queries
{
    public record CustomerCommentGetDto
    {
        public string BillId { get; set; }
        public string Comment { get; set; }
        public string UserDisplayName { get; set; }
        public Guid UserId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string InsertDateJalali { get; set; }
    }
}
