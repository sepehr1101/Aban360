using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;

public interface IUploaderService
{
    Task CreateAsync(UploaderCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(UploaderDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(UploaderUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<UploaderGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<UploaderGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class UploaderService : IUploaderService
{
    private readonly IMapper _mapper;
    private readonly IUploaderCommandService _commands;
    private readonly IUploaderQueryService _queries;

    public UploaderService(IMapper mapper, IUploaderCommandService commands, IUploaderQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(UploaderCreateDto createDto, CancellationToken cancellationToken)
    {
        var uploader = _mapper.Map<Uploader>(createDto);
        await _commands.Add(uploader);
    }

    public async Task DeleteAsync(UploaderDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var uploader = await _queries.Get(deleteDto.Id);
        await _commands.Remove(uploader);
    }

    public async Task UpdateAsync(UploaderUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var uploader = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, uploader);
    }

    public async Task<ICollection<UploaderGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var uploader = await _queries.Get();
        return _mapper.Map<ICollection<UploaderGetDto>>(uploader);
    }

    public async Task<UploaderGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var uploader = await _queries.Get(id);
        return _mapper.Map<UploaderGetDto>(uploader);
    }
}
