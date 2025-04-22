using Aban360.Common.ApplicationUser;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Create.Contracts
{
    public interface IDynamicReportCreateHandler
    {
        Task Handle(IAppUser currentUser, string name, string reportTemplateJson);
    }
}
