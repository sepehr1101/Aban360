using Aban360.Common.ApplicationUser;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Commands.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Hangfire;

namespace Aban360.Api.Cronjobs
{
    public interface IReportGenerator
    {
        Task DirectExecute<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId);
    }

    internal sealed class ReportGenerator : IReportGenerator
    {
        private readonly IServerReportsCreateHandler _serverReportsCreateHandler;
        private readonly IServerReportsUpdateHandler _serverReportsUpdateHandler;
        public ReportGenerator(
            IServerReportsCreateHandler serverReportsCreateHandler,
            IServerReportsUpdateHandler serverReportsUpdateHandler)
        {
            _serverReportsCreateHandler = serverReportsCreateHandler;
            _serverReportsCreateHandler.NotNull(nameof(serverReportsCreateHandler));

            _serverReportsUpdateHandler = serverReportsUpdateHandler;
            _serverReportsUpdateHandler.NotNull(nameof(serverReportsUpdateHandler));
        }
        public async Task DirectExecute<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId)
        {
            Guid id = Guid.NewGuid();
            ServerReportsCreateDto serverReportsCreateDto = CreateServerReportDto(reportInput, GetData, appUser, reportTitle, connectionId);
            _serverReportsCreateHandler.Handle(serverReportsCreateDto, cancellationToken);


            //Sample:  await GenerateReports.FireAndInform(inputDto, cancellationToken, _emptyUnit.Handle);

            //Insert ServerReport
            ReportOutput<THead, TData> reportOutput = await GetData(reportInput, cancellationToken);
            string reportPath = await ExcelManagement.ExportToExcelAsync(reportOutput.ReportHeader, reportOutput.ReportData, reportOutput.Title);

            //Complete ServerReport
            _serverReportsUpdateHandler.Handle(new ServerReportsUpdateDto(id, reportPath), cancellationToken);

            //send events via signalR
        }
        public void FireAndInform<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId)
        {
            //BackgroundJob.Enqueue(job =>
            //       job.ExecuteAsync<TReportInput, THead, TData>(
            //           jobId,
            //           reportInput,
            //           userId,
            //           reportTitle,
            //           connectionId,
            //           null! // Cancellation token not passed
            //       )
            //);
            //BackgroundJob.Enqueue((x,y,z)=>)
        }
        private ServerReportsCreateDto CreateServerReportDto<TReportInput, THead, TData>(TReportInput reportInput, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId)
        {
            ServerReportsCreateDto serverReportsCreateDto = new()
            {

            };
            return serverReportsCreateDto;
        }
    }
}
