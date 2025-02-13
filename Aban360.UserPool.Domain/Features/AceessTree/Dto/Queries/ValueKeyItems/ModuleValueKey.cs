namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems
{
    public record ModuleValueKey:AccessTreeBaseValueKey
    {
        public ICollection<SubModuleValueKey> SubModuleValueKeys { get; set; }
        public ModuleValueKey(int id, string title, string style, ICollection<SubModuleValueKey> subModuleValueKeys)
            :base(id, title, style)
        {
            SubModuleValueKeys = subModuleValueKeys;
        }
    }
}
