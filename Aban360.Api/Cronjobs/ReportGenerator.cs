using Aban360.Common.ApplicationUser;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Commands.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;

namespace Aban360.Api.Cronjobs
{
    public interface IReportGenerator
    {
        Task FireAndInform<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle,string connectionId);
    }

    internal sealed class ReportGenerator : IReportGenerator
    {
        private readonly IServerReportsCreateHandler _serverReportsCreateHandler;
        public ReportGenerator(IServerReportsCreateHandler serverReportsCreateHandler)
        {
            _serverReportsCreateHandler = serverReportsCreateHandler;
            _serverReportsCreateHandler.NotNull(nameof(serverReportsCreateHandler));
        }
        public async Task FireAndInform<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId)
        {
             _serverReportsCreateHandler.Handle( new ServerReportsCreateDto(appUser.UserId,reportTitle, connectionId), cancellationToken);


            //Sample:  await GenerateReports.FireAndInform(inputDto, cancellationToken, _emptyUnit.Handle);

            //Insert ServerReport
            ReportOutput<THead, TData> reportOutput = await GetData(reportInput, cancellationToken);
            await ExcelManagement.ExportToExcelAsync(reportOutput.ReportHeader, reportOutput.ReportData, reportOutput.Title);

            //Complete ServerReport

            //send events via signalR
        }
    }
}
