namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface IDeleteBillIdTagHandler
    {
        Task<bool> Handle(long id);
    }
}