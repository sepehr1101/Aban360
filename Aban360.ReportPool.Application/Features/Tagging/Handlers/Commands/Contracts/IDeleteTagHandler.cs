namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface IDeleteTagHandler
    {
        Task<bool> Handle(int id);
    }
}