namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface IDeleteTagGroupHandler
    {
        Task<bool> Handle(int id);
    }
}