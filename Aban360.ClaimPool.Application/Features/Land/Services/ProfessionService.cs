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

public interface IProfessionService
{
    Task<ProfessionGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<ProfessionGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(ProfessionUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(ProfessionDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(ProfessionCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ProfessionService : IProfessionService
{
    private readonly IMapper _mapper;
    private readonly IProfessionQueryService _queryService;
    private readonly IValidator<ProfessionUpdateDto> _updateValidator;
    private readonly IProfessionCommandService _commandService;
    private readonly IValidator<ProfessionCreateDto> _createValidator;

    public ProfessionService(IMapper mapper, IProfessionQueryService queryService, IValidator<ProfessionUpdateDto> updateValidator, IProfessionCommandService commandService, IValidator<ProfessionCreateDto> createValidator)
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

    public async Task<ProfessionGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        Profession profession = await _queryService.Get(id);
        if (profession == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ProfessionGetDto>(profession);
    }

    public async Task<ICollection<ProfessionGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Profession> profession = await _queryService.Get();
        if (profession == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<ProfessionGetDto>>(profession);
    }

    public async Task UpdateAsync(ProfessionUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        Profession profession = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, profession);
    }

    public async Task DeleteAsync(ProfessionDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Profession profession = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(profession);
    
    }

    public async Task CreateAsync(ProfessionCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        Profession profession = _mapper.Map<Profession>(createDto);
        await _commandService.Add(profession);
    }

}
