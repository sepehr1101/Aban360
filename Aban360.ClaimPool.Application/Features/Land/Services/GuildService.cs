using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Services;

public interface IGuildService
{
    Task<GuildGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<GuildGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(GuildUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(GuildDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(GuildCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class GuildService : IGuildService
{
    private readonly IMapper _mapper;
    private readonly IGuildQueryService _queryService;
    private readonly IValidator<GuildUpdateDto> _updateValidator;
    private readonly IGuildCommandService _commandService;
    private readonly IValidator<GuildCreateDto> _createValidator;

    public GuildService(IMapper mapper, IGuildQueryService queryService, IValidator<GuildUpdateDto> updateValidator, IGuildCommandService commandService, IValidator<GuildCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queryService.NotNull(nameof(queryService));
        _queryService = queryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commandService.NotNull(nameof(commandService));
        _commandService = commandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<GuildGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        Guild guild = await _queryService.Get(id);
        return _mapper.Map<GuildGetDto>(guild);
    }

    public async Task<ICollection<GuildGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Guild> guild = await _queryService.Get();
        if (guild == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<GuildGetDto>>(guild);
    }

    public async Task UpdateAsync(GuildUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        Guild guild = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, guild);
    }

    public async Task DeleteAsync(GuildDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Guild guild = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(guild);
    
    }

    public async Task CreateAsync(GuildCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        Guild guild = _mapper.Map<Guild>(createDto);
        await _commandService.Add(guild);
    }

}
