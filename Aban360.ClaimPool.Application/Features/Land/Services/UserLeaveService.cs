using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Services;

public interface IUserLeaveService
{
    Task<UserLeaveGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<UserLeaveGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UserLeaveUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(UserLeaveDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(UserLeaveCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class UserLeaveService : IUserLeaveService
{
    private readonly IMapper _mapper;
    private readonly IUserLeaveQueryService _queries;
    private readonly IValidator<UserLeaveUpdateDto> _updateValidator;
    private readonly IUserLeaveCommandService _commands;
    private readonly IValidator<UserLeaveCreateDto> _createValidator;

    public UserLeaveService(IMapper mapper, IUserLeaveQueryService queries, IValidator<UserLeaveUpdateDto> updateValidator, IUserLeaveCommandService commands, IValidator<UserLeaveCreateDto> createValidator)
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

    public async Task<UserLeaveGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var userLeave = await _queries.Get(id);
        return _mapper.Map<UserLeaveGetDto>(userLeave);
    }

    public async Task<ICollection<UserLeaveGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var userLeave = await _queries.Get();
        return _mapper.Map<ICollection<UserLeaveGetDto>>(userLeave);
    }

    public async Task UpdateAsync(UserLeaveUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var userLeave = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, userLeave);
    }

    public async Task DeleteAsync(UserLeaveDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var userLeave = await _queries.Get(deleteDto.Id);
        await _commands.Remove(userLeave);
    }

    public async Task CreateAsync(UserLeaveCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var userLeave = _mapper.Map<UserLeave>(createDto);
        await _commands.Add(userLeave);
    }
}
