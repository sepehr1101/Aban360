namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys
{
    public record BaseLocationItemValueKey
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public BaseLocationItemValueKey(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
