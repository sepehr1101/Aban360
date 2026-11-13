using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestWaterMeterService
{
    Task UpdateAsync(IAppUser currentUser, WaterMeterRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WaterMeterRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IAppUser currentUser, WaterMeterRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestWaterMeterService : IRequestWaterMeterService
{
    private readonly IMapper _mapper;
    private readonly IRequestWaterMeterQueryService _requestWaterMeterQueryService;
    private readonly IValidator<WaterMeterRequestUpdateDto> _updateValidator;
    private readonly IRequestWaterMeterCommandService _requestWaterMeterCommandService;
    private readonly IRequestWaterMeterTagCommandService _requestWaterMeterTagCommandService;
    private readonly IRequestWaterMeterTagQueryService _requestWaterMeterTagQueryService;
    private readonly IValidator<WaterMeterRequestCreateDto> _createValidator;

    public RequestWaterMeterService(IMapper mapper, IRequestWaterMeterQueryService requestWaterMeterQueryService, IValidator<WaterMeterRequestUpdateDto> updateValidator, IRequestWaterMeterCommandService requestWaterMeterCommandService, IRequestWaterMeterTagCommandService requestWaterMeterTagCommandService, IRequestWaterMeterTagQueryService requestWaterMeterTagQueryService, IValidator<WaterMeterRequestCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        requestWaterMeterQueryService.NotNull(nameof(requestWaterMeterQueryService));
        _requestWaterMeterQueryService = requestWaterMeterQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        requestWaterMeterCommandService.NotNull(nameof(requestWaterMeterCommandService));
        _requestWaterMeterCommandService = requestWaterMeterCommandService;
        requestWaterMeterTagCommandService.NotNull(nameof(requestWaterMeterTagCommandService));
        _requestWaterMeterTagCommandService = requestWaterMeterTagCommandService;
        requestWaterMeterTagQueryService.NotNull(nameof(requestWaterMeterTagQueryService));
        _requestWaterMeterTagQueryService = requestWaterMeterTagQueryService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task UpdateAsync(IAppUser currentUser, WaterMeterRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var requestWaterMeter = await _requestWaterMeterQueryService.Get(updateDto.Id);
        requestWaterMeter.Hash = "-";
        requestWaterMeter.InsertLogInfo = "-";
        requestWaterMeter.ValidFrom = DateTime.Now;
        requestWaterMeter.UserId = currentUser.UserId;
    
        _mapper.Map(updateDto, requestWaterMeter);
    }

    public async Task DeleteAsync(WaterMeterRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        ICollection<RequestWaterMeterTag> requestWaterMeterTags = await _requestWaterMeterTagQueryService.GetByWaterMeterId(deleteDto.Id);
        if (requestWaterMeterTags != null)
        {
            _requestWaterMeterTagCommandService.Remove(requestWaterMeterTags);
        }
    
        var requestWaterMeter = await _requestWaterMeterQueryService.Get(deleteDto.Id);
        _requestWaterMeterCommandService.Remove(requestWaterMeter);
    }

    public async Task CreateAsync(IAppUser currentUser, WaterMeterRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var requestWaterMeter = _mapper.Map<RequestWaterMeter>(createDto);
        requestWaterMeter.Hash = "-";
        requestWaterMeter.InsertLogInfo = "-";
        requestWaterMeter.ValidFrom = DateTime.Now;
        requestWaterMeter.UserId = currentUser.UserId;
    
        createDto.TagIds.ForEach(tags =>
        {
            RequestWaterMeterTag requestWaterMeterTag = new RequestWaterMeterTag()
            {
                RequestWaterMeter = requestWaterMeter,
                WaterMeterTagDefinitionId = tags,
                Hash = "-",
                InsertLogInfo = "-",
                ValidFrom = DateTime.Now,
            };
            requestWaterMeter.WaterMeterTags.Add(requestWaterMeterTag);
        });
    
        await _requestWaterMeterCommandService.Add(requestWaterMeter);
    }

}
