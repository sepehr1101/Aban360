using Aban360.Common.ApplicationUser;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Commands.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;
using Hangfire;
using Newtonsoft.Json;

namespace Aban360.Api.Cronjobs
{
    public interface IReportGenerator
    {
        Task DirectExecute<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId);
        Task FireAndInform<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId);
    }

    internal sealed class ReportGenerator : IReportGenerator
    {
        private readonly IServerReportsCreateHandler _serverReportsCreateHandler;
        private readonly IServerReportsUpdateHandler _serverReportsUpdateHandler;
        private readonly IServerReportsGetByIdService _serverReportsGetByIdServices;
        private readonly IServiceProvider _serviceProvider;
        private static string MethodName = "Handle";

        public ReportGenerator(
            IServerReportsCreateHandler serverReportsCreateHandler,
            IServerReportsUpdateHandler serverReportsUpdateHandler,
            IServerReportsGetByIdService serverReportsGetByIdServices,
            IServiceProvider serviceProvider)
        {
            _serverReportsCreateHandler = serverReportsCreateHandler;
            _serverReportsCreateHandler.NotNull(nameof(serverReportsCreateHandler));

            _serverReportsUpdateHandler = serverReportsUpdateHandler;
            _serverReportsUpdateHandler.NotNull(nameof(serverReportsUpdateHandler));

            _serverReportsGetByIdServices = serverReportsGetByIdServices;
            _serverReportsGetByIdServices.NotNull(nameof(serverReportsGetByIdServices));

            _serviceProvider = serviceProvider;
            _serviceProvider.NotNull(nameof(serviceProvider));
        }
        public async Task DirectExecute<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId)
        {
            Guid id = Guid.NewGuid();
            ServerReportsCreateDto serverReportsCreateDto = CreateServerReportDto(id, reportInput, GetData, appUser, reportTitle, connectionId);
            await _serverReportsCreateHandler.Handle(serverReportsCreateDto, cancellationToken);


            //Sample:  await GenerateReports.FireAndInform(inputDto, cancellationToken, _emptyUnit.Handle);

            //Insert ServerReport
            ReportOutput<THead, TData> reportOutput = await GetData(reportInput, cancellationToken);
            string reportPath = await ExcelManagement.ExportToExcelAsync(reportOutput.ReportHeader, reportOutput.ReportData, reportOutput.Title);

            //Complete ServerReport
            _serverReportsUpdateHandler.Handle(new ServerReportsUpdateDto(id, reportPath), cancellationToken);

            //send events via signalR
        }
        public async Task FireAndInform<TReportInput, THead, TData>(TReportInput reportInput, CancellationToken cancellationToken, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId)
        {
            ServerReportsCreateDto serverReportsCreateDto = CreateServerReportDto(Guid.NewGuid(), reportInput, GetData, appUser, reportTitle, connectionId);
            await _serverReportsCreateHandler.Handle(serverReportsCreateDto, cancellationToken);
            BackgroundJob.Enqueue(() => DoFireAndInform(serverReportsCreateDto));
        }

       public async Task DoFireAndInform(ServerReportsCreateDto serverReportsCreateDto )
        {
            ServerReportsGetByIdDto serverReportsGetByIdDto = await _serverReportsGetByIdServices.GetById(serverReportsCreateDto.Id);

            var interfaceType = AppDomain.CurrentDomain
             .GetAssemblies()
             .SelectMany(a => a.GetTypes())
             .FirstOrDefault(t => t.IsInterface && t.FullName == serverReportsGetByIdDto.HandlerKey)
                 ?? throw new Exception($"Interface '{serverReportsGetByIdDto.HandlerKey}' not found.");

            var handlerInstance = _serviceProvider.GetRequiredService(interfaceType);

            var inputDtoType = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == serverReportsGetByIdDto.ReportInputType);

            var outputHeaderDtoType = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == serverReportsGetByIdDto.HeaderType);

            var outputDataDtoType = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == serverReportsGetByIdDto.DataType);

            object? data = System.Text.Json.JsonSerializer.Deserialize(serverReportsGetByIdDto.ReportInputJson, inputDtoType);
            var methodName = interfaceType.GetMethod(MethodName);

            var reportOutputType = typeof(ReportOutput<,>).MakeGenericType(outputHeaderDtoType, outputDataDtoType);

            var result = methodName.Invoke(handlerInstance, [ data,CancellationToken.None ]) as Task;
            await result;

            var resultProperty = result.GetType().GetProperty("Result");
            var actualResult = resultProperty?.GetValue(result);

            if (actualResult != null && reportOutputType.IsInstanceOfType(actualResult))
            {
                dynamic dynamicResult = actualResult;
                var reportHeader = dynamicResult.ReportHeader;
                var reportData = dynamicResult.ReportData;

                string reportPath = await ExcelManagement.ExportToExcelAsync(reportHeader, reportData, serverReportsGetByIdDto.ReportName);
                _serverReportsUpdateHandler.Handle(new ServerReportsUpdateDto(serverReportsGetByIdDto.Id, reportPath), CancellationToken.None);
            }
            //inform user via signalR
        }

        private ServerReportsCreateDto CreateServerReportDto<TReportInput, THead, TData>(Guid id, TReportInput reportInput, Func<TReportInput, CancellationToken, Task<ReportOutput<THead, TData>>> GetData, IAppUser appUser, string reportTitle, string connectionId)
        {
            ServerReportsCreateDto serverReportsCreateDto = new()
            {
                Id = id,
                UserId = appUser.UserId,
                ReportName = reportTitle,
                ConnectionId = connectionId,
                HeaderType = typeof(THead).ToString(),
                DataType = typeof(TData).ToString(),
                ReportInputType = typeof(TReportInput).ToString(),
                ReportInputJson = JsonConvert.SerializeObject(reportInput),
                HandlerKey = GetData.Target.GetType().GetInterfaces().FirstOrDefault().FullName
            };
            return serverReportsCreateDto;
        }
    }
}
