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

public interface IOfficialHolidayService
{
    Task<OfficialHolidayGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<OfficialHolidayGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(OfficialHolidayUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(OfficialHolidayDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(OfficialHolidayCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class OfficialHolidayService : IOfficialHolidayService
{
    private readonly IMapper _mapper;
    private readonly IOfficialHolidayQueryService _queries;
    private readonly IValidator<OfficialHolidayUpdateDto> _updateValidator;
    private readonly IOfficialHolidayCommandService _commands;
    private readonly IValidator<OfficialHolidayCreateDto> _createValidator;

    public OfficialHolidayService(IMapper mapper, IOfficialHolidayQueryService queries, IValidator<OfficialHolidayUpdateDto> updateValidator, IOfficialHolidayCommandService commands, IValidator<OfficialHolidayCreateDto> createValidator)
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

    public async Task<OfficialHolidayGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var officialHoliday = await _queries.Get(id);
        return _mapper.Map<OfficialHolidayGetDto>(officialHoliday);
    }

    public async Task<ICollection<OfficialHolidayGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var officialHoliday = await _queries.Get();
        return _mapper.Map<ICollection<OfficialHolidayGetDto>>(officialHoliday);
    }

    public async Task UpdateAsync(OfficialHolidayUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var officialHoliday = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, officialHoliday);
    }

    public async Task DeleteAsync(OfficialHolidayDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var officialHoliday = await _queries.Get(deleteDto.Id);
        await _commands.Remove(officialHoliday);
    }

    public async Task CreateAsync(OfficialHolidayCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var officialHoliday = _mapper.Map<OfficialHoliday>(createDto);
        await _commands.Add(officialHoliday);
    }
}
