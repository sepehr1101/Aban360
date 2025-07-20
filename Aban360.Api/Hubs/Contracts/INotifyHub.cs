namespace Aban360.Api.Hubs.Contracts
{
    public interface INotifyHub
    {
        Task BroadcastMessage(string caption, string message);
        Task InformReportCompletion(string message);
    }
}
