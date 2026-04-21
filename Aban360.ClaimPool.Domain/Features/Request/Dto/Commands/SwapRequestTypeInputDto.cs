namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record SwapRequestTypeInputDto
    {
        public int TrackNumber { get; set; }
        public ICollection<int> SelectedServices { get; set; }
    }
}
