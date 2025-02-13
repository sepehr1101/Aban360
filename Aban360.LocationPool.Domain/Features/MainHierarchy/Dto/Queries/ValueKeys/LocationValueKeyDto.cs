namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys
{
    public record LocationValueKeyDto
    {
        public ICollection<CordinalDirectionValueKey> CordinalDirectionValueKeys { get; set; } = default!;
        public LocationValueKeyDto(ICollection<CordinalDirectionValueKey> cordinalDirectionValueKeys)
        {
            CordinalDirectionValueKeys = cordinalDirectionValueKeys;
        }
    }
}
