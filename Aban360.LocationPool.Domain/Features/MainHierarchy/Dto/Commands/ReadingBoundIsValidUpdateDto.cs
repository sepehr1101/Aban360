namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands
{
    public record ReadingBoundIsValidUpdateDto
    {
        public int Id { get; set; }
        public bool IsValid { get; set; }
    }
}
