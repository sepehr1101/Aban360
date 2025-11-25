namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IMeterLifeInsertHandler
    {
        Task Handle(CancellationToken cancellationToken);
    }
}
