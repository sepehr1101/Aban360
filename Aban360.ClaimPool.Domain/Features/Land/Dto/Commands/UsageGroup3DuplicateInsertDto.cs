namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageGroup3DuplicateInsertDto
    {
        public short PreviousGroup2Id { get; set; }
        public short NewGroup2Id { get; set; }
        public UsageGroup3DuplicateInsertDto(short previousGroup2Id, short newGroup2Id)
        {
            PreviousGroup2Id = previousGroup2Id;
            NewGroup2Id = newGroup2Id;
        }
        public UsageGroup3DuplicateInsertDto()
        {
        }
    }
}
