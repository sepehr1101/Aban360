namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys
{
    public record ProvinceValueKey : BaseLocationItemValueKey
    {
        public ICollection<HeadquartersValueKey> HeadquartersValueKeys { get; set; } = default!;
        public ProvinceValueKey(int id, string title, ICollection<HeadquartersValueKey> headquartersValueKeys)
            :base(id, title)
        {
            HeadquartersValueKeys=headquartersValueKeys;
        }
    }
}
