namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts
{
    public interface IDynamicReportGetTemplateJsonHandler
    {
        Task<string> Handle(int id);
    }
}