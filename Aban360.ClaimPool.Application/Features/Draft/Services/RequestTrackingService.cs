using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestTrackingService
{
    Task<ICollection<RequestTrackingGetDto>> GetByWaterMeterIdAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<RequestTrackingGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(RequestTrackingUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync( RequestTrackingDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IAppUser currentUser,RequestTrackingCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestTrackingService : IRequestTrackingService
{
    private readonly IMapper _mapper;
    private readonly IRequestTrackingQueryService _requestTrackingQueryService;
    private readonly IRequestTrackingCommandService _requestTrackingCommandService;

    public RequestTrackingService(IMapper mapper, IRequestTrackingQueryService requestTrackingQueryService, IRequestTrackingCommandService requestTrackingCommandService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        requestTrackingQueryService.NotNull(nameof(requestTrackingQueryService));
        _requestTrackingQueryService = requestTrackingQueryService;
        requestTrackingCommandService.NotNull(nameof(requestTrackingCommandService));
        _requestTrackingCommandService = requestTrackingCommandService;
    }

    public async Task<ICollection<RequestTrackingGetDto>> GetByWaterMeterIdAsync(int id, CancellationToken cancellationToken)
    {
        ICollection<RequestTracking> requestTracking = await _requestTrackingQueryService.GetByWaterMeterId(id);
        return _mapper.Map<ICollection<RequestTrackingGetDto>>(requestTracking);
    }

    public async Task<ICollection<RequestTrackingGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var requestTracking = await _requestTrackingQueryService.Get();
        return _mapper.Map<ICollection<RequestTrackingGetDto>>(requestTracking);
    }

    public async Task UpdateAsync(RequestTrackingUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var requestTracking = await _requestTrackingQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, requestTracking);
    }

    public async Task DeleteAsync( RequestTrackingDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var requestTracking = await _requestTrackingQueryService.Get(deleteDto.Id);
        requestTracking.RemoveLogInfo = "--";
    
        await _requestTrackingCommandService.Remove(requestTracking);
    }

    public async Task CreateAsync(IAppUser currentUser,RequestTrackingCreateDto createDto, CancellationToken cancellationToken)
    {
        RequestTracking requestTracking = _mapper.Map<RequestTracking>(createDto);
        requestTracking.UserId=currentUser.UserId;
        requestTracking.ValidFrom = DateTime.Now;
        requestTracking.Hash = "--";
        requestTracking.InsertLogInfo = "--";
    
        await _requestTrackingCommandService.Add(requestTracking);
    }

}
