using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.Api.Cronjobs
{
    internal static class ReportGenerator
    {
        internal static async Task FireAndInform<TReportInput,THead,TData>(TReportInput reportInput,CancellationToken cancellationToken,Func<TReportInput, CancellationToken,Task<ReportOutput<THead,TData>>> GetData)
        {
            //Sample:  await GenerateReports.FireAndInform(inputDto, cancellationToken, _emptyUnit.Handle);

            //Insert ServerReport
            ReportOutput<THead, TData> reportOutput= await GetData(reportInput,cancellationToken);
            await ExcelManagement.ExportToExcelAsync(reportOutput.ReportHeader, reportOutput.ReportData, reportOutput.Title);

            //Complete ServerReport

            //send events via signalR
        }
    }
}
