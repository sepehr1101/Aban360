namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualTypeGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
