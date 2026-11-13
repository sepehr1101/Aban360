using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;

public interface IBankFileStructureService
{
    Task CreateAsync(BankFileStructureCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(BankFileStructureDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(BankFileStructureUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<BankFileStructureGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<BankFileStructureGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class BankFileStructureService : IBankFileStructureService
{
    private readonly IMapper _mapper;
    private readonly IBankFileStructureCommandService _commands;
    private readonly IBankFileStructureQueryService _queries;

    public BankFileStructureService(IMapper mapper, IBankFileStructureCommandService commands, IBankFileStructureQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(BankFileStructureCreateDto createDto, CancellationToken cancellationToken)
    {
        var bankFileStructure = _mapper.Map<BankFileStructure>(createDto);
        await _commands.Add(bankFileStructure);
    }

    public async Task DeleteAsync(BankFileStructureDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var bankFileStructure = await _queries.Get(deleteDto.Id);
        await _commands.Remove(bankFileStructure);
    }

    public async Task UpdateAsync(BankFileStructureUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var bankFileStructure = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, bankFileStructure);
    }

    public async Task<ICollection<BankFileStructureGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var bankFileStructure = await _queries.Get();
        return _mapper.Map<ICollection<BankFileStructureGetDto>>(bankFileStructure);
    }

    public async Task<BankFileStructureGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var bankFileStructure = await _queries.Get(id);
        return _mapper.Map<BankFileStructureGetDto>(bankFileStructure);
    }
}
