namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems
{
    public record AppValueKey: AccessTreeBaseValueKey
    {
        public ICollection<ModuleValueKey> ModuleValueKeys { get; set; }
        public AppValueKey(int id, string title, string style, ICollection<ModuleValueKey> moduleValueKeys)
            :base(id, title, style)
        {
            ModuleValueKeys= moduleValueKeys;
        }
    }
}
