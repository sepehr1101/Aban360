namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record IndividualTagCommandDto
    {
        //  public int Id { get; set; }
        public int IndividualId { get; set; }
        public short IndividualTagDefinitionId { get; set; }
        public string? Value { get; set; }
    }
}
