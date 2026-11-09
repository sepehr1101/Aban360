using Aban360.CommunicationPool.Domain.Constants;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;

namespace Aban360.Api.Hubs.Implementations
{
    public sealed class HubPresenceHeartbeatService : BackgroundService
    {
        private readonly HubConnectionRegistry _connections;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<HubPresenceHeartbeatService> _logger;

        public HubPresenceHeartbeatService(HubConnectionRegistry connections, IServiceScopeFactory scopeFactory,
            ILogger<HubPresenceHeartbeatService> logger)
        {
            _connections = connections;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(HubPresenceConstants.HeartbeatIntervalSeconds));
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    try
                    {
                        using var refreshTimeout = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                        refreshTimeout.CancelAfter(TimeSpan.FromSeconds(HubPresenceConstants.HeartbeatTimeoutSeconds));
                        await RefreshAsync(refreshTimeout.Token);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "Could not refresh SignalR connection presence.");
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { }
        }

        public async Task RefreshAsync(CancellationToken cancellationToken)
        {
            var connectionIds = _connections.GetConnectionIds();
            if (connectionIds.Length == 0) return;
            using var scope = _scopeFactory.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IHubPresenceService>().Refresh(connectionIds, cancellationToken);
        }
    }
}
