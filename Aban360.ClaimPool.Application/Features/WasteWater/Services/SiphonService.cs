using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Services;

public interface ISiphonService
{
    Task<SiphonGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<SiphonGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(SiphonUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SiphonDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(SiphonCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class SiphonService : ISiphonService
{
    private readonly IMapper _mapper;
    private readonly ISiphonQueryService _queryService;
    private readonly IValidator<SiphonUpdateDto> _updateValidator;
    private readonly ISiphonCommandService _commandService;
    private readonly IWaterMeterQueryService _meterQueryService;
    private readonly IWaterMeterSiphonCommandService _meterSiphonCommandService;
    private readonly IValidator<SiphonCreateDto> _createValidator;

    public SiphonService(IMapper mapper, ISiphonQueryService queryService, IValidator<SiphonUpdateDto> updateValidator, ISiphonCommandService commandService, IWaterMeterQueryService meterQueryService, IWaterMeterSiphonCommandService meterSiphonCommandService, IValidator<SiphonCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queryService.NotNull(nameof(queryService));
        _queryService = queryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commandService.NotNull(nameof(commandService));
        _commandService = commandService;
        meterQueryService.NotNull(nameof(meterQueryService));
        _meterQueryService = meterQueryService;
        meterSiphonCommandService.NotNull(nameof(meterSiphonCommandService));
        _meterSiphonCommandService = meterSiphonCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<SiphonGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Siphon siphon = await _queryService.Get(id);
        return _mapper.Map<SiphonGetDto>(siphon);
    }

    public async Task<ICollection<SiphonGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Siphon> siphon = await _queryService.Get();
        return _mapper.Map<ICollection<SiphonGetDto>>(siphon);
    }

    public async Task UpdateAsync(SiphonUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message=string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
            throw new BaseException(message);
        }
    
        Siphon siphon = await _queryService.Get(updateDto.Id);
        siphon.ValidFrom = DateTime.Now;
        siphon.InsertLogInfo = "SampleLogInfo";
        siphon.Hash = "SmapleHash";
    
        _mapper.Map(updateDto, siphon);
    }

    public async Task DeleteAsync(SiphonDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Siphon siphon = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(siphon);
    }

    public async Task CreateAsync(SiphonCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    
        Siphon siphon = _mapper.Map<Siphon>(createDto);
        siphon.ValidFrom = DateTime.Now;
        siphon.InsertLogInfo = "SampleLogInfo";
        siphon.Hash = "SmapleHash";
    
        WaterMeter waterMeter = await _meterQueryService.Get(createDto.WaterMeterId);
        WaterMeterSiphon waterMeterSiphon = new WaterMeterSiphon()
        {
            WaterMeterId = waterMeter.Id,
            Siphon = siphon
        };
        //await _commandService.Add(siphon);
        await _meterSiphonCommandService.Add(waterMeterSiphon);
    }

}
