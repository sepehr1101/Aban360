namespace Aban360.Api.Hubs.Contracts
{
    public interface IReportCompletionNotifier
    {
        Task Notify(Guid userId, string reportTitle, Guid reportId);
    }
}
