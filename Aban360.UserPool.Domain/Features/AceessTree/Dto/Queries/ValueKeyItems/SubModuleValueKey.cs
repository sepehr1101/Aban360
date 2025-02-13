namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems
{
    public record SubModuleValueKey: AccessTreeBaseValueKey
    {
        public ICollection<EndpointValueKey> EndpointValueKeys { get; set; }
        public SubModuleValueKey(int id, string title, string style, ICollection<EndpointValueKey> endpointValueKeys)
            :base(id,title, style)
        {
            EndpointValueKeys = endpointValueKeys;
        }
    }
}
