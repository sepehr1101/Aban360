using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Services;

public interface IBankService
{
    Task<ICollection<BankGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<BankGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class BankService : IBankService
{
    private readonly IMapper _mapper;
    private readonly IBankQueryService _queries;

    public BankService(IMapper mapper, IBankQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        queries.NotNull(nameof(queries));
        _mapper = mapper;
        _queries = queries;
    }

    public async Task<ICollection<BankGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = await _queries.Get();
        return _mapper.Map<ICollection<BankGetDto>>(items);
    }

    public async Task<BankGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var item = await _queries.Get(id);
        return _mapper.Map<BankGetDto>(item);
    }
}
