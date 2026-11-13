using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;

public interface ICreditService
{
    Task<CreditGetDto> GetSingleAsync(long id, CancellationToken cancellationToken);
    Task<ICollection<CreditGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(CreditUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(CreditDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class CreditService : ICreditService
{
    private readonly IMapper _mapper;
    private readonly ICreditQueryService _queries;
    private readonly ICreditCommandService _commands;

    public CreditService(IMapper mapper, ICreditQueryService queries, ICreditCommandService commands)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        commands.NotNull(nameof(commands));
        _commands = commands;
    }

    public async Task<CreditGetDto> GetSingleAsync(long id, CancellationToken cancellationToken)
    {
        var credit = await _queries.Get(id);
        return _mapper.Map<CreditGetDto>(credit);
    }

    public async Task<ICollection<CreditGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var credit = await _queries.Get();
        return _mapper.Map<ICollection<CreditGetDto>>(credit);
    }

    public async Task UpdateAsync(CreditUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var credit = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, credit);
    }

    public async Task DeleteAsync(CreditDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var credit = await _queries.Get(deleteDto.Id);
        await _commands.Remove(credit);
    }
}
