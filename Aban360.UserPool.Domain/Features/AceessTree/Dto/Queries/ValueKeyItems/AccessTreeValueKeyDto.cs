namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems
{
    public record AccessTreeValueKeyDto
    {
        public ICollection<AppValueKey> AppValueKeys { get; set; } = default!;
        public AccessTreeValueKeyDto(ICollection<AppValueKey> appValueKeys)
        {
            AppValueKeys = appValueKeys;
        }
    }
}
