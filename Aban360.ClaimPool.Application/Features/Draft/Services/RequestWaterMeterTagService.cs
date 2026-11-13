using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestWaterMeterTagService
{
    Task UpdateAsync(WaterMeterTagRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WaterMeterTagRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(WaterMeterTagRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestWaterMeterTagService : IRequestWaterMeterTagService
{
    private readonly IMapper _mapper;
    private readonly IRequestWaterMeterTagQueryService _queries;
    private readonly IValidator<WaterMeterTagRequestUpdateDto> _updateValidator;
    private readonly IRequestWaterMeterTagCommandService _commands;
    private readonly IValidator<WaterMeterTagRequestCreateDto> _createValidator;

    public RequestWaterMeterTagService(IMapper mapper, IRequestWaterMeterTagQueryService queries, IValidator<WaterMeterTagRequestUpdateDto> updateValidator, IRequestWaterMeterTagCommandService commands, IValidator<WaterMeterTagRequestCreateDto> createValidator)
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

    public async Task UpdateAsync(WaterMeterTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestWaterMeterTag = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, requestWaterMeterTag);
    }

    public async Task DeleteAsync(WaterMeterTagRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var requestWaterMeterTag = await _queries.Get(deleteDto.Id);
        await _commands.Remove(requestWaterMeterTag);
    }

    public async Task CreateAsync(WaterMeterTagRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestWaterMeterTag = _mapper.Map<RequestWaterMeterTag>(createDto);
        requestWaterMeterTag .Hash = "-";
        requestWaterMeterTag.InsertLogInfo = "-";
        requestWaterMeterTag.ValidFrom = DateTime.Now;

        await _commands.Add(requestWaterMeterTag);
    }
}
