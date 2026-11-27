namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts
{
    public interface IZoneTitleAddhoc
    {
        Task<string> Handle(int id, CancellationToken cancellationToken);
    }
}
