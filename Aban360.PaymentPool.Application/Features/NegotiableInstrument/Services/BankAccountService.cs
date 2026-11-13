using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;
using System.Threading;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;

public interface IBankAccountService
{
    Task<BankAccountGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<BankAccountGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(BankAccountUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(BankAccountDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(BankAccountCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class BankAccountService : IBankAccountService
{
    private readonly IMapper _mapper;
    private readonly IBankAccountQueryService _queries;
    private readonly IRegionTitleAddhoc _regionTitleAddhoc;
    private readonly IBankAccountCommandService _commands;

    public BankAccountService(IMapper mapper, IBankAccountQueryService queries, IRegionTitleAddhoc regionTitleAddhoc, IBankAccountCommandService commands)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        regionTitleAddhoc.NotNull(nameof(regionTitleAddhoc));
        _regionTitleAddhoc = regionTitleAddhoc;
        commands.NotNull(nameof(commands));
        _commands = commands;
    }

    public async Task<BankAccountGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var BankAccount = await _queries.Get(id);
        return _mapper.Map<BankAccountGetDto>(BankAccount);
    }

    public async Task<ICollection<BankAccountGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var bankAccount = await _queries.Get();
        return _mapper.Map<ICollection<BankAccountGetDto>>(bankAccount);
    }

    public async Task UpdateAsync(BankAccountUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var bankAccount = await _queries.Get(updateDto.Id);
        bankAccount.RegionTitle = await _regionTitleAddhoc.Handle(updateDto.RegionId, cancellationToken);

        _mapper.Map(updateDto, bankAccount);
    }

    public async Task DeleteAsync(BankAccountDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var bankAccount = await _queries.Get(deleteDto.Id);
        await _commands.Remove(bankAccount);
    }

    public async Task CreateAsync(BankAccountCreateDto createDto, CancellationToken cancellationToken)
    {
        var bankAccount = _mapper.Map<BankAccount>(createDto);
        bankAccount.RegionTitle = await _regionTitleAddhoc.Handle(createDto.RegionId, cancellationToken);

        await _commands.Add(bankAccount);
    }
}
