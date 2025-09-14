namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface IDeleteTagGroupHandler
    {
        Task<bool> Handle(int id);
    }
}