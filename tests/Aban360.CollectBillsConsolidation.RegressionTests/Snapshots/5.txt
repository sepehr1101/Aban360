using Aban360.CalculationPool.Application.Features.Base;
using Aban360.Common.Extensions;
using Hangfire;

namespace Aban360.Api.Cronjobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 3600)]
    public class CollectBillsJob
    {
        private readonly ICollectBillsDetailJobService _collectBillsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<CollectBillsJob> _logger;
        private int _cancelAfterMin = 10;
        public CollectBillsJob(
            ICollectBillsDetailJobService collectBillsService,
            IWebHostEnvironment webHostEnvironment,
            ILogger<CollectBillsJob> logger)
        {
            _collectBillsService = collectBillsService;
            _collectBillsService.NotNull(nameof(collectBillsService));

            _webHostEnvironment = webHostEnvironment;
            _webHostEnvironment.NotNull(nameof(webHostEnvironment));

            _logger = logger;
            _logger.NotNull(nameof(logger));

        }

        public async Task RunAsync()
        {
            if (!_webHostEnvironment.IsDevelopment())
            {
                _logger.LogInformation("CollectBills Job Started.");
                using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(_cancelAfterMin));
                await _collectBillsService.Initialize(DateTime.Now.AddDays(-1), cts.Token);
                _logger.LogInformation("CollectBills Job Ended.");
            }
        }
    }
}
