namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys
{
    public record HeadquartersValueKey : BaseLocationItemValueKey
    {       
        public ICollection<RegionValueKey> RegionValueKeys { get; set; } = default!;
        public HeadquartersValueKey(int id, string title, ICollection<RegionValueKey> regionValueKeys)
            :base(id, title)
        {
            RegionValueKeys=regionValueKeys;
        }
    }
}
