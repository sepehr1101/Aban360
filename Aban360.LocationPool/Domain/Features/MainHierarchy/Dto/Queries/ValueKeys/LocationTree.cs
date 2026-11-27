namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys
{
    public record LocationTree
    {
        public ICollection<CordinalDirectionValueKey> CordinalDirectionValueKeys { get; set; } = default!;
        public LocationTree(ICollection<CordinalDirectionValueKey> cordinalDirectionValueKeys)
        {
            CordinalDirectionValueKeys = cordinalDirectionValueKeys;
        }
    }
}
