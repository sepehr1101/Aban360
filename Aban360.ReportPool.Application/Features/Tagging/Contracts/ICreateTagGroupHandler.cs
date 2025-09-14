using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface ICreateTagGroupHandler
    {
        Task<int> Handle(CreateTagGroupDto dto);
    }
}