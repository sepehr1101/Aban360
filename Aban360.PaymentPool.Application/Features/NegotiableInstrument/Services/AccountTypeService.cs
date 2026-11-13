using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;

public interface IAccountTypeService
{
    Task<ICollection<AccountTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<AccountTypeGetDto> GetSingleAsync(AccountTypeEnum id, CancellationToken cancellationToken);
}

internal sealed class AccountTypeService : IAccountTypeService
{
    private readonly IMapper _mapper;
    private readonly IAccountTypeQueryService _queries;

    public AccountTypeService(IMapper mapper, IAccountTypeQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        queries.NotNull(nameof(queries));
        _mapper = mapper;
        _queries = queries;
    }

    public async Task<ICollection<AccountTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = await _queries.Get();
        return _mapper.Map<ICollection<AccountTypeGetDto>>(items);
    }

    public async Task<AccountTypeGetDto> GetSingleAsync(AccountTypeEnum id, CancellationToken cancellationToken)
    {
        var item = await _queries.Get(id);
        return _mapper.Map<AccountTypeGetDto>(item);
    }
}
