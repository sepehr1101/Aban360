namespace Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts
{
    public interface IZoneAddHoc
    {
        Task<bool> GetArticle11(int zoneId);
    }
}
