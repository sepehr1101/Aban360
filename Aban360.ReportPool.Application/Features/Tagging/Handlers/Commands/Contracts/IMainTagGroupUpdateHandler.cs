using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface IMainTagGroupUpdateHandler
    {
        Task Handle(MainTagGroupUpdateDto input);
    }
}
