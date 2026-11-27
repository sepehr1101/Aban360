using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;

public interface ICreditorTypeService
{
    Task<ICollection<CreditorTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<CreditorTypeGetDto> GetSingleAsync(CreditorTypeEnum id, CancellationToken cancellationToken);
}

internal sealed class CreditorTypeService : ICreditorTypeService
{
    private readonly IMapper _mapper;
    private readonly ICreditorTypeQueryService _queries;

    public CreditorTypeService(IMapper mapper, ICreditorTypeQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        queries.NotNull(nameof(queries));
        _mapper = mapper;
        _queries = queries;
    }

    public async Task<ICollection<CreditorTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = await _queries.Get();
        return _mapper.Map<ICollection<CreditorTypeGetDto>>(items);
    }

    public async Task<CreditorTypeGetDto> GetSingleAsync(CreditorTypeEnum id, CancellationToken cancellationToken)
    {
        var item = await _queries.Get(id);
        return _mapper.Map<CreditorTypeGetDto>(item);
    }
}
