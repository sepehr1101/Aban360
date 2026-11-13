using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestSiphonService
{
    Task UpdateAsync(IAppUser currentUser, SiphonRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IAppUser currentUser, SiphonRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestSiphonService : IRequestSiphonService
{
    private readonly IMapper _mapper;
    private readonly IRequestSiphonQueryService _requestSiphonQueryService;
    private readonly IValidator<SiphonRequestUpdateDto> _updateValidator;
    private readonly IRequestSiphonCommandService _requestSiphonCommandService;
    private readonly IRequestWaterMeterSiphonQueryService _requestWaterMeterSiphonQueryService;
    private readonly IRequestWaterMeterSiphonCommandService _requestWaterMeterSiphonCommandService;
    private readonly IValidator<SiphonRequestCreateDto> _createValidator;

    public RequestSiphonService(IMapper mapper, IRequestSiphonQueryService requestSiphonQueryService, IValidator<SiphonRequestUpdateDto> updateValidator, IRequestSiphonCommandService requestSiphonCommandService, IRequestWaterMeterSiphonQueryService requestWaterMeterSiphonQueryService, IRequestWaterMeterSiphonCommandService requestWaterMeterSiphonCommandService, IValidator<SiphonRequestCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        requestSiphonQueryService.NotNull(nameof(requestSiphonQueryService));
        _requestSiphonQueryService = requestSiphonQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        requestSiphonCommandService.NotNull(nameof(requestSiphonCommandService));
        _requestSiphonCommandService = requestSiphonCommandService;
        requestWaterMeterSiphonQueryService.NotNull(nameof(requestWaterMeterSiphonQueryService));
        _requestWaterMeterSiphonQueryService = requestWaterMeterSiphonQueryService;
        requestWaterMeterSiphonCommandService.NotNull(nameof(requestWaterMeterSiphonCommandService));
        _requestWaterMeterSiphonCommandService = requestWaterMeterSiphonCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task UpdateAsync(IAppUser currentUser, SiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var requestSiphon = await _requestSiphonQueryService.Get(updateDto.Id);
        requestSiphon.Hash = "-";
        requestSiphon.InsertLogInfo = "-";
        requestSiphon.ValidFrom = DateTime.Now;
        requestSiphon.UserId = currentUser.UserId;
    
        _mapper.Map(updateDto, requestSiphon);
    }

    public async Task DeleteAsync(SiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        RequestWaterMeterSiphon requestWaterMeterSiphon = await _requestWaterMeterSiphonQueryService.GetBySiphonId(deleteDto.Id);
        if (requestWaterMeterSiphon != null)
        {
            _requestWaterMeterSiphonCommandService.Remove(requestWaterMeterSiphon);
        }
        var requestSiphon = await _requestSiphonQueryService.Get(deleteDto.Id);
        _requestSiphonCommandService.Remove(requestSiphon);
    }

    public async Task CreateAsync(IAppUser currentUser, SiphonRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        RequestSiphon requestSiphon = _mapper.Map<RequestSiphon>(createDto);
        requestSiphon.Hash = "-";
        requestSiphon.InsertLogInfo = "-";
        requestSiphon.ValidFrom = DateTime.Now;
        requestSiphon.UserId = currentUser.UserId;
    
        RequestWaterMeterSiphon requestWaterMeterSiphon = new RequestWaterMeterSiphon()
        {
            RequestSiphon = requestSiphon,
            WaterMeterId=createDto.WaterMeterId
        };
    
        //await _requestSiphonCommandService.Add(requestSiphon);
        await _requestWaterMeterSiphonCommandService.Add(requestWaterMeterSiphon);
    }

}
