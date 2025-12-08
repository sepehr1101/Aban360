using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Hangfire;

namespace Aban360.Api.Cronjobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 3600)]
    public class MeterLifeJob
    {
        const int cancelAfterMin = 5;
        private readonly IMeterLifeInsertHandler _handler;
        private readonly ILogger<MeterLifeJob> _logger;

        public MeterLifeJob(IMeterLifeInsertHandler handler, ILogger<MeterLifeJob> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("MeterLife job started.");
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(cancelAfterMin));
            await _handler.Handle(cts.Token);
            _logger.LogInformation("MeterLife job ended.");
        }
    }
}
