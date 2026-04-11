namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record CustomerCommentInputDto
    {
        public string BillId { get; set; }
        public string Comment { get; set; }
    }
}
