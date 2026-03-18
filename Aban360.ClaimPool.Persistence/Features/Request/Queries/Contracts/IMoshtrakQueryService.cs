namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IMoshtrakQueryService
    {
        Task CheckOpenRequest(string nationalCode, int zoneId);
        Task CheckOpenRequest(int customerNumber, int zoneId);
    }
}
