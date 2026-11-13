using Aban360.Common.Extensions;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Services;

public interface ISupportedFieldService
{
    Task<ICollection<SupportedFieldGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<SupportedFieldGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class SupportedFieldService : ISupportedFieldService
{
    private readonly IMapper _mapper;
    private readonly ISupportedFieldQueryService _queries;

    public SupportedFieldService(IMapper mapper, ISupportedFieldQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        queries.NotNull(nameof(queries));
        _mapper = mapper;
        _queries = queries;
    }

    public async Task<ICollection<SupportedFieldGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = await _queries.Get();
        return _mapper.Map<ICollection<SupportedFieldGetDto>>(items);
    }

    public async Task<SupportedFieldGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var item = await _queries.Get(id);
        return _mapper.Map<SupportedFieldGetDto>(item);
    }
}
