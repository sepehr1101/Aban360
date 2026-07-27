using Aban360.ReportPool.Domain.Features.Tagging.Commands;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface ICreateTagGroupHandler
    {
        Task<int> Handle(CreateTagGroupDto dto);
    }
}