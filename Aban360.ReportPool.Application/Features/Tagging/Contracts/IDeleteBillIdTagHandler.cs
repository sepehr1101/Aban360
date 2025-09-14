namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface IDeleteBillIdTagHandler
    {
        Task<bool> Handle(long id);
    }
}