namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys
{
    public record ZoneValueKey : BaseLocationItemValueKey
    {
        public bool IsSelected { get; set; }
        public ZoneValueKey(int id, string title, bool isSelected=false) : base(id, title)
        {
            IsSelected = isSelected;
        }     
    }
}
