namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems
{
    public record EndpointValueKey:AccessTreeBaseValueKey
    {
        public bool IsSelected { get; set; }
        public EndpointValueKey(int id, string title, string style, bool isSelected=false)
            :base(id,title, style)
        {
            IsSelected = isSelected;
        }
    }
}
