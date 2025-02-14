namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualEstateUpdateDto
    {
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public int EstateId { get; set; }
        public short IndividualEstateRelationTypeId { get; set; }
    }
}
