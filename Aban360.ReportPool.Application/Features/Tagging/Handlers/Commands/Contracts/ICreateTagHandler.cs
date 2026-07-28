using Aban360.ReportPool.Domain.Features.Tagging.Commands;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface ICreateTagHandler
    {
        Task<int> Handle(CreateTagDto dto);
    }
}