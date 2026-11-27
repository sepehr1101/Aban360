namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts
{
    public interface IRegionTitleAddhoc
    {
        Task<string> Handle(int id, CancellationToken cancellationToken);
    }
}
