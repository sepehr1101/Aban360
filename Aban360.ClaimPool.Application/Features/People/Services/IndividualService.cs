using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Services;

public interface IIndividualService
{
    Task<IndividualGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<IndividualGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(IndividualUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class IndividualService : IIndividualService
{
    private readonly IMapper _mapper;
    private readonly IIndividualQueryService _queryService;
    private readonly IValidator<IndividualUpdateDto> _updateValidator;
    private readonly IIndividualCommandService _commandService;
    private readonly IEstateQueryService _estateQueryService;
    private readonly IIndividualEstateCommandService _individualEstateCommandService;
    private readonly IValidator<IndividualCreateDto> _createValidator;

    public IndividualService(IMapper mapper, IIndividualQueryService queryService, IValidator<IndividualUpdateDto> updateValidator, IIndividualCommandService commandService, IEstateQueryService estateQueryService, IIndividualEstateCommandService individualEstateCommandService, IValidator<IndividualCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queryService.NotNull(nameof(queryService));
        _queryService = queryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commandService.NotNull(nameof(commandService));
        _commandService = commandService;
        estateQueryService.NotNull(nameof(estateQueryService));
        _estateQueryService = estateQueryService;
        individualEstateCommandService.NotNull(nameof(individualEstateCommandService));
        _individualEstateCommandService = individualEstateCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<IndividualGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Individual individual = await _queryService.Get(id);
        return _mapper.Map<IndividualGetDto>(individual);
    }

    public async Task<ICollection<IndividualGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Individual> individual = await _queryService.Get();
        return _mapper.Map<ICollection<IndividualGetDto>>(individual);
    }

    public async Task UpdateAsync(IndividualUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message=string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage) );
            throw new BaseException(message);
        }
    
        Individual individual = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, individual);
    }

    public async Task DeleteAsync(IndividualDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Individual individual = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(individual);
    }

    public async Task CreateAsync(IndividualCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult=await _createValidator .ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message=string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
            throw new BaseException(message);
        }
    
        Individual individual = _mapper.Map<Individual>(createDto);
        individual.Hash = " "; //todo Hash in not null
    
        Estate estate = await _estateQueryService.Get(createDto.EstateId);
        IndividualEstate individualEstate = new IndividualEstate()
        {
            EstateId=estate.Id,
            Individual=individual,
            IndividualEstateRelationTypeId=createDto.IndividualEstateRelationTypeId,
        };
        //await _commandService.Add(individual);
        await _individualEstateCommandService.Add(individualEstate);
    }

}
