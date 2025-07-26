using Newtonsoft.Json;
using System.Reflection;

namespace Aban360.Api.Cronjobs
{
    public class ReportJobDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly IAppUserResolver _appUserResolver;

        public ReportJobDispatcher(IServiceProvider serviceProvider/*, IAppUserResolver appUserResolver*/)
        {
            _serviceProvider = serviceProvider;
            //_appUserResolver = appUserResolver;
        }

        public async Task DispatchAsync(ReportJobArgs args)
        {
            var reportInputType = Type.GetType(args.ReportInputType);
            var headType = Type.GetType(args.HeadType);
            var dataType = Type.GetType(args.DataType);

            if (reportInputType == null || headType == null || dataType == null)
                throw new InvalidOperationException("Type resolution failed.");

            var method = typeof(ReportJobDispatcher)
                .GetMethod(nameof(HandleReportAsync), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(reportInputType, headType, dataType);

            await (Task)method.Invoke(this, new object[] { args });
        }

        private async Task HandleReportAsync<TReportInput, THead, TData>(ReportJobArgs args)
        {
            var reportInput = JsonConvert.DeserializeObject<TReportInput>(args.ReportInputJson);
            //var appUser = await _appUserResolver.ResolveAsync(args.UserId);

            //var reportHandler = _serviceProvider.GetRequiredService<IReportGenerator<TReportInput, THead, TData>>();

            //var output = await reportHandler.GenerateAsync(reportInput, appUser, args.ReportTitle, args.ConnectionId);

            // Save to DB, push via SignalR, etc.
        }
    }

}
