namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualTagDefinitionUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Color { get; set; }
    }
}
