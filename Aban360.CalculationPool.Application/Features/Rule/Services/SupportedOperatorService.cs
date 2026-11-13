using Aban360.Common.Extensions;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Services;

public interface ISupportedOperatorService
{
    Task<ICollection<SupportedOperatorGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<SupportedOperatorGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class SupportedOperatorService : ISupportedOperatorService
{
    private readonly IMapper _mapper;
    private readonly ISupportedOperatorQueryService _queries;

    public SupportedOperatorService(IMapper mapper, ISupportedOperatorQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        queries.NotNull(nameof(queries));
        _mapper = mapper;
        _queries = queries;
    }

    public async Task<ICollection<SupportedOperatorGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = await _queries.Get();
        return _mapper.Map<ICollection<SupportedOperatorGetDto>>(items);
    }

    public async Task<SupportedOperatorGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var item = await _queries.Get(id);
        return _mapper.Map<SupportedOperatorGetDto>(item);
    }
}
