using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Services;
public interface IEstateService
{
    Task CreateAsync(EstateCreateDto createDto, CancellationToken cancellationToken);
    Task UpdateAsync(EstateUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(EstateDeleteDto deleteDto, CancellationToken cancellationToken);
    Task<ICollection<EstateGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<EstateGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
}

internal sealed class EstateService : IEstateService
{
    private readonly IMapper _mapper;
    private readonly IEstateCommandService _commandService;
    private readonly IValidator<EstateCreateDto> _estateValidator;
    private readonly IEstateQueryService _queryService;
    private readonly IValidator<EstateUpdateDto> _updateValidator;
    public EstateService(IMapper mapper, IEstateCommandService commandService, IValidator<EstateCreateDto> estateValidator, IEstateQueryService queryService, IValidator<EstateUpdateDto> updateAsyncEstateValidator)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(_mapper));
        _commandService = commandService;
        _commandService.NotNull(nameof(_commandService));
        _estateValidator = estateValidator;
        _estateValidator.NotNull(nameof(estateValidator));
        _mapper.NotNull(nameof(mapper));
        _queryService = queryService;
        _queryService.NotNull(nameof(queryService));
        _updateValidator = updateAsyncEstateValidator;
        _updateValidator.NotNull(nameof(updateAsyncEstateValidator));
    }

    public async Task CreateAsync(EstateCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _estateValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }

        Estate estate = _mapper.Map<Estate>(createDto);
        estate.ValidFrom = DateTime.Now;
        estate.InsertLogInfo = "loginfo";
        estate.Hash = "hash";
        await _commandService.Add(estate);
    }

    public async Task UpdateAsync(EstateUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }

        Estate estate = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, estate);
    }

    public async Task DeleteAsync(EstateDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Estate estate = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(estate);
    }

    public async Task<ICollection<EstateGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Estate> estate = await _queryService.Get();
        if (estate == null)
        {
            throw new InvalidDataException();
        }

        return _mapper.Map<ICollection<EstateGetDto>>(estate);
    }

    public async Task<EstateGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Estate estate = await _queryService.Get(id);
        if (estate == null)
        {
            throw new InvalidDataException();
        }

        return _mapper.Map<EstateGetDto>(estate);
    }
}
