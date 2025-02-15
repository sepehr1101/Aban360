namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts
{
    public interface IZoneCountQueryAddhoc
    {
        Task<int> GetCount(ICollection<int> input, CancellationToken cancellationToken);
    }
}
