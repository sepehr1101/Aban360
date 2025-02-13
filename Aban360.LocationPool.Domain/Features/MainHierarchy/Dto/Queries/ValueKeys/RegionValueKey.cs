namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys
{
    public record RegionValueKey : BaseLocationItemValueKey
    {        
        public ICollection<ZoneValueKey> ZoneValueKeys { get; set; } = default!;
        public RegionValueKey(int id, string title, ICollection<ZoneValueKey> zoneValueKeys)
            :base(id, title)
        {
            ZoneValueKeys = zoneValueKeys;
        }
    }
}
