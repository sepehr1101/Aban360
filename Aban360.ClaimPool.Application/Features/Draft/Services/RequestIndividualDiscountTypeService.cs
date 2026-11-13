using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestIndividualDiscountTypeService
{
    Task<RequestIndividualDiscountTypeGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<RequestIndividualDiscountTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(RequestIndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(RequestIndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(RequestIndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestIndividualDiscountTypeService : IRequestIndividualDiscountTypeService
{
    private readonly IMapper _mapper;
    private readonly IRequestIndividualDiscountTypeQueryService _queries;
    private readonly IValidator<RequestIndividualDiscountTypeUpdateDto> _updateValidator;
    private readonly IRequestIndividualDiscountTypeCommandService _commands;
    private readonly IValidator<RequestIndividualDiscountTypeCreateDto> _createValidator;

    public RequestIndividualDiscountTypeService(IMapper mapper, IRequestIndividualDiscountTypeQueryService queries, IValidator<RequestIndividualDiscountTypeUpdateDto> updateValidator, IRequestIndividualDiscountTypeCommandService commands, IValidator<RequestIndividualDiscountTypeCreateDto> createValidator)
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

    public async Task<RequestIndividualDiscountTypeGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        var individualDiscountType = await _queries.Get(id);
        return _mapper.Map<RequestIndividualDiscountTypeGetDto>(individualDiscountType);
    }

    public async Task<ICollection<RequestIndividualDiscountTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var individualDiscountType = await _queries.Get();
        return _mapper.Map<ICollection<RequestIndividualDiscountTypeGetDto>>(individualDiscountType);
    }

    public async Task UpdateAsync(RequestIndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
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

    public async Task DeleteAsync(RequestIndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var individualDiscountType = await _queries.Get(deleteDto.Id);
        await _commands.Remove(individualDiscountType);
    }

    public async Task CreateAsync(RequestIndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var individualDiscountType = _mapper.Map<RequestIndividualDiscountType>(createDto);
        individualDiscountType.Hash = "hash";
        individualDiscountType.ValidFrom = DateTime.Now;
        individualDiscountType.InsertLogInfo = "insertLogInfo";

        await _commands.Add(individualDiscountType);
    }
}
