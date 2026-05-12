using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Hangfire;

namespace Aban360.Api.Cronjobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 3600)]
    public class MeterLifeJob
    {
        const int cancelAfterMin = 5;
        private readonly IMeterLifeInsertHandler _handler;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<MeterLifeJob> _logger;

        public MeterLifeJob(
            IMeterLifeInsertHandler handler,
            ILogger<MeterLifeJob> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _handler = handler;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task RunAsync()
        {
            if (!_webHostEnvironment.IsDevelopment())//todo: check
            {
                _logger.LogInformation("MeterLife job started.");
                using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(cancelAfterMin));
                await _handler.Handle(cts.Token);
                _logger.LogInformation("MeterLife job ended.");
            }
        }
    }
}
