using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Services;

public interface IDiscountTypeService
{
    Task<ICollection<DiscountTypeGetDto>> GetAllAsync(bool isReturn, CancellationToken cancellationToken);
    Task<DiscountTypeGetDto> GetSingleAsync(DiscountTypeEnum id, CancellationToken cancellationToken);
}

internal sealed class DiscountTypeService : IDiscountTypeService
{
    private readonly IMapper _mapper;
    private readonly IDiscountTypeQueryService _queries;
    private readonly DiscountTypeEnum _noDiscountTypeId = DiscountTypeEnum.BedonTakhfif;

    public DiscountTypeService(IMapper mapper, IDiscountTypeQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task<ICollection<DiscountTypeGetDto>> GetAllAsync(bool isReturn, CancellationToken cancellationToken)
    {
        var discountTypes = await _queries.Get();
        ICollection<DiscountTypeGetDto> result = _mapper.Map<ICollection<DiscountTypeGetDto>>(discountTypes);

        return isReturn ? result.Where(k => k.Id != _noDiscountTypeId).ToList() : result;
    }

    public async Task<DiscountTypeGetDto> GetSingleAsync(DiscountTypeEnum id, CancellationToken cancellationToken)
    {
        var discountType = await _queries.Get(id);
        return _mapper.Map<DiscountTypeGetDto>(discountType);
    }
}
