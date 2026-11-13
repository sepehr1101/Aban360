using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Request.Services;

public interface IGatewayService
{
    Task<GatewayGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<GatewayGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(GatewayUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(GatewayDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(GatewayCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class GatewayService : IGatewayService
{
    private readonly IMapper _mapper;
    private readonly IGatewayQueryService _queries;
    private readonly IValidator<GatewayUpdateDto> _updateValidator;
    private readonly IGatewayCommandService _commands;
    private readonly IValidator<GatewayCreateDto> _createValidator;

    public GatewayService(IMapper mapper, IGatewayQueryService queries, IValidator<GatewayUpdateDto> updateValidator, IGatewayCommandService commands, IValidator<GatewayCreateDto> createValidator)
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

    public async Task<GatewayGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        Gateway geteway = await _queries.Get(id);
        return _mapper.Map<GatewayGetDto>(geteway);
    }

    public async Task<ICollection<GatewayGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Gateway> geteway = await _queries.Get();
        return _mapper.Map<ICollection<GatewayGetDto>>(geteway);
    }

    public async Task UpdateAsync(GatewayUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Gateway gateway = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, gateway);
    }

    public async Task DeleteAsync(GatewayDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Gateway geteway = await _queries.Get(deleteDto.Id);
        await _commands.Remove(geteway);
    }

    public async Task CreateAsync(GatewayCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Gateway geteway = _mapper.Map<Gateway>(createDto);
        await _commands.Add(geteway);
    }
}
