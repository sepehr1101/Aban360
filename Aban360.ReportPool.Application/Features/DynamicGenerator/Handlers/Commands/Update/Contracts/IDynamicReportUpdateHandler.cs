using Aban360.Common.ApplicationUser;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Update.Contracts
{
    public interface IDynamicReportUpdateHandler
    {
        Task Handle(IAppUser currentUser, int id, string name, string reportTemplateJson);
    }
}
