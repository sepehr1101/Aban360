using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;
using FluentValidation;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Services;

public interface IHubEventService
{
    Task UpdateAsync(HubEventUpdateDto input, CancellationToken cancellationToken);
    Task CreateAsync(HubEventCreateDto input, CancellationToken cancellationToken);
}

internal sealed class HubEventService : IHubEventService
{
    private readonly IHubEventUpdateService _hubEventUpdateService;
    private readonly IHubEventCreateService _hubEventCreateService;

    public HubEventService(IHubEventUpdateService hubEventUpdateService, IHubEventCreateService hubEventCreateService)
    {
        hubEventUpdateService.NotNull(nameof(hubEventUpdateService));
        _hubEventUpdateService = hubEventUpdateService;
        hubEventCreateService.NotNull(nameof(hubEventCreateService));
        _hubEventCreateService = hubEventCreateService;
    }

    public async Task UpdateAsync(HubEventUpdateDto input, CancellationToken cancellationToken)
    {
        await _hubEventUpdateService.CloseConnection(input);
    }

    public async Task CreateAsync(HubEventCreateDto input, CancellationToken cancellationToken)
    {
        await _hubEventCreateService.Create(input);
    }
}
