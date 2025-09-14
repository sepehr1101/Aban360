namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface IDeleteTagHandler
    {
        Task<bool> Handle(int id);
    }
}