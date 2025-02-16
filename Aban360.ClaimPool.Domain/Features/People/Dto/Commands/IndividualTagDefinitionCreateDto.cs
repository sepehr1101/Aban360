namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualTagDefinitionCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Color { get; set; }
    }
}
