using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Services;

public interface IUserWorkdayService
{
    Task<UserWorkdayGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<UserWorkdayGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UserWorkdayUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(UserWorkdayDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(UserWorkdayCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class UserWorkdayService : IUserWorkdayService
{
    private readonly IMapper _mapper;
    private readonly IUserWorkdayQueryService _userWorkdayQueryService;
    private readonly IZoneTitleAddhoc _zoneTitleAddhoc;
    private readonly IValidator<UserWorkdayUpdateDto> _updateValidator;
    private readonly IUserWorkdayCommandService _userWorkdayCommandService;
    private readonly IValidator<UserWorkdayCreateDto> _createValidator;

    public UserWorkdayService(IMapper mapper, IUserWorkdayQueryService userWorkdayQueryService, IZoneTitleAddhoc zoneTitleAddhoc, IValidator<UserWorkdayUpdateDto> updateValidator, IUserWorkdayCommandService userWorkdayCommandService, IValidator<UserWorkdayCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        userWorkdayQueryService.NotNull(nameof(userWorkdayQueryService));
        _userWorkdayQueryService = userWorkdayQueryService;
        zoneTitleAddhoc.NotNull(nameof(zoneTitleAddhoc));
        _zoneTitleAddhoc = zoneTitleAddhoc;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        userWorkdayCommandService.NotNull(nameof(userWorkdayCommandService));
        _userWorkdayCommandService = userWorkdayCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<UserWorkdayGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var userWorkday = await _userWorkdayQueryService.Get(id);
        return _mapper.Map<UserWorkdayGetDto>(userWorkday);
    }

    public async Task<ICollection<UserWorkdayGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var userWorkday = await _userWorkdayQueryService.Get();
        return _mapper.Map<ICollection<UserWorkdayGetDto>>(userWorkday);
    }

    public async Task UpdateAsync(UserWorkdayUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var userWorkday = await _userWorkdayQueryService.Get(updateDto.Id);
        var zoneTitle = await _zoneTitleAddhoc.Handle(updateDto.ZoneId, cancellationToken);
    
        userWorkday.ZoneTitle = zoneTitle;
        _mapper.Map(updateDto, userWorkday);
    }

    public async Task DeleteAsync(UserWorkdayDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var userWorkday = await _userWorkdayQueryService.Get(deleteDto.Id);
        await _userWorkdayCommandService.Remove(userWorkday);
    }

    public async Task CreateAsync(UserWorkdayCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var userWorkday = _mapper.Map<UserWorkday>(createDto);
        var zoneTitle=await _zoneTitleAddhoc.Handle(createDto.ZoneId,cancellationToken);
    
        userWorkday.ZoneTitle = zoneTitle;
        await _userWorkdayCommandService.Add(userWorkday);
    }

}
