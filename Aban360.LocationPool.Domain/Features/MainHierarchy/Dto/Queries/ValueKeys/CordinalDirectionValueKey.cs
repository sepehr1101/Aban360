namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys
{
    public record CordinalDirectionValueKey : BaseLocationItemValueKey
    {      
        public ICollection<ProvinceValueKey> ProvinceValueKeys { get; set; } = default!;
        public CordinalDirectionValueKey(int id, string title, ICollection<ProvinceValueKey> provinceValueKeys)
            :base(id, title)
        {
            ProvinceValueKeys=provinceValueKeys;
        }
    }
}
