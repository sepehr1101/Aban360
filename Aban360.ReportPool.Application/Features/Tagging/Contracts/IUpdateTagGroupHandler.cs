using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface IUpdateTagGroupHandler
    {
        Task<bool> Handle(UpdateTagGroupDto dto);
    }
}