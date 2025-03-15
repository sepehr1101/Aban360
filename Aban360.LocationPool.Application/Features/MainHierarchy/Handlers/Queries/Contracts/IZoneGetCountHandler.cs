namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts
{
    public interface IZoneGetCountHandler
    {
        Task<int> Handle(ICollection<int> input, CancellationToken cancellationToken);
    }


}
