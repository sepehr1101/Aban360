using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestWaterMeterSiphonService
{
    Task UpdateAsync(WaterMeterSiphonRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WaterMeterSiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(WaterMeterSiphonRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestWaterMeterSiphonService : IRequestWaterMeterSiphonService
{
    private readonly IMapper _mapper;
    private readonly IRequestWaterMeterSiphonQueryService _queries;
    private readonly IValidator<WaterMeterSiphonRequestUpdateDto> _updateValidator;
    private readonly IRequestWaterMeterSiphonCommandService _commands;
    private readonly IValidator<WaterMeterSiphonRequestCreateDto> _createValidator;

    public RequestWaterMeterSiphonService(IMapper mapper, IRequestWaterMeterSiphonQueryService queries, IValidator<WaterMeterSiphonRequestUpdateDto> updateValidator, IRequestWaterMeterSiphonCommandService commands, IValidator<WaterMeterSiphonRequestCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commands.NotNull(nameof(commands));
        _commands = commands;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task UpdateAsync(WaterMeterSiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestWaterMeterSiphon = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, requestWaterMeterSiphon);
    }

    public async Task DeleteAsync(WaterMeterSiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var requestWaterMeterSiphon = await _queries.Get(deleteDto.Id);
        _commands.Remove(requestWaterMeterSiphon);
    }

    public async Task CreateAsync(WaterMeterSiphonRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestWaterMeterSiphon = _mapper.Map<RequestWaterMeterSiphon>(createDto);
        await _commands.Add(requestWaterMeterSiphon);
    }
}
