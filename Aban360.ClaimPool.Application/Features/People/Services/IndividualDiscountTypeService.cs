using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.People.Services;

public interface IIndividualDiscountTypeService
{
    Task<IndividualDiscountTypeGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<IndividualDiscountTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(IndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class IndividualDiscountTypeService : IIndividualDiscountTypeService
{
    private readonly IMapper _mapper;
    private readonly IIndividualDiscountTypeQueryService _queries;
    private readonly IValidator<IndividualDiscountTypeUpdateDto> _updateValidator;
    private readonly IIndividualDiscountTypeCommandService _commands;
    private readonly IValidator<IndividualDiscountTypeCreateDto> _createValidator;

    public IndividualDiscountTypeService(IMapper mapper, IIndividualDiscountTypeQueryService queries, IValidator<IndividualDiscountTypeUpdateDto> updateValidator, IIndividualDiscountTypeCommandService commands, IValidator<IndividualDiscountTypeCreateDto> createValidator)
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

    public async Task<IndividualDiscountTypeGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        var individualDiscountType = await _queries.Get(id);
        return _mapper.Map<IndividualDiscountTypeGetDto>(individualDiscountType);
    }

    public async Task<ICollection<IndividualDiscountTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var individualDiscountType = await _queries.Get();
        return _mapper.Map<ICollection<IndividualDiscountTypeGetDto>>(individualDiscountType);
    }

    public async Task UpdateAsync(IndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var individualDiscountType = await _queries.Get(updateDto.Id);
        individualDiscountType.Hash = "hash";
        individualDiscountType.ValidFrom = DateTime.Now;
        individualDiscountType.InsertLogInfo = "insertLogInfo";

        _mapper.Map(updateDto, individualDiscountType);
    }

    public async Task DeleteAsync(IndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var individualDiscountType = await _queries.Get(deleteDto.Id);
        await _commands.Remove(individualDiscountType);
    }

    public async Task CreateAsync(IndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var individualDiscountType = _mapper.Map<IndividualDiscountType>(createDto);
        individualDiscountType.Hash = "hash";
        individualDiscountType.ValidFrom = DateTime.Now;
        individualDiscountType.InsertLogInfo = "insertLogInfo";

        await _commands.Add(individualDiscountType);
    }
}
