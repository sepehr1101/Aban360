using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Services;

public interface IPaymentMethodService
{
    Task<ICollection<PaymentMethodGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<PaymentMethodGetDto> GetSingleAsync(PaymentMethodEnum id, CancellationToken cancellationToken);
}

internal sealed class PaymentMethodService : IPaymentMethodService
{
    private readonly IMapper _mapper;
    private readonly IPaymentMethodQueryService _queries;

    public PaymentMethodService(IMapper mapper, IPaymentMethodQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task<ICollection<PaymentMethodGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var bank = await _queries.Get();
        return _mapper.Map<ICollection<PaymentMethodGetDto>>(bank);
    }

    public async Task<PaymentMethodGetDto> GetSingleAsync(PaymentMethodEnum id, CancellationToken cancellationToken)
    {
        var bank = await _queries.Get(id);
        return _mapper.Map<PaymentMethodGetDto>(bank);
    }
}
