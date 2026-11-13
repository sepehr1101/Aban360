using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Services;

public interface ISiphonDiameterService
{
    Task<SiphonDiameterGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<SiphonDiameterGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(SiphonDiameterUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SiphonDiameterDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(SiphonDiameterCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class SiphonDiameterService : ISiphonDiameterService
{
    private readonly IMapper _mapper;
    private readonly ISiphonDiameterQueryService _queries;
    private readonly IValidator<SiphonDiameterUpdateDto> _updateValidator;
    private readonly ISiphonDiameterCommandService _commands;
    private readonly IValidator<SiphonDiameterCreateDto> _createValidator;

    public SiphonDiameterService(IMapper mapper, ISiphonDiameterQueryService queries, IValidator<SiphonDiameterUpdateDto> updateValidator, ISiphonDiameterCommandService commands, IValidator<SiphonDiameterCreateDto> createValidator)
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

    public async Task<SiphonDiameterGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        SiphonDiameter siphonDiameter = await _queries.Get(id);
        return _mapper.Map<SiphonDiameterGetDto>(siphonDiameter);
    }

    public async Task<ICollection<SiphonDiameterGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<SiphonDiameter> siphonDiameter = await _queries.Get();
        return _mapper.Map<ICollection<SiphonDiameterGetDto>>(siphonDiameter);
    }

    public async Task UpdateAsync(SiphonDiameterUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        SiphonDiameter siphonDiameter = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, siphonDiameter);
    }

    public async Task DeleteAsync(SiphonDiameterDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        SiphonDiameter siphonDiameter = await _queries.Get(deleteDto.Id);
        await _commands.Remove(siphonDiameter);
    }

    public async Task CreateAsync(SiphonDiameterCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        SiphonDiameter SiphonDiameter = _mapper.Map<SiphonDiameter>(createDto);
        await _commands.Add(SiphonDiameter);
    }
}
