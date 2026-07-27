using Aban360.ReportPool.Domain.Features.Tagging.Commands;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface IMainTagGroupCreateHandler
    {
        Task Handle(MainTagGroupInsertInputDto input);
    }
}
