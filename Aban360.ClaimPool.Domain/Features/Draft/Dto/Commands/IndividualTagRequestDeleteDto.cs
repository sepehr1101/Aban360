namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record IndividualTagRequestDeleteDto : IndividualTagCommandDto
    {
        public int Id { get; set; }
    }
}
