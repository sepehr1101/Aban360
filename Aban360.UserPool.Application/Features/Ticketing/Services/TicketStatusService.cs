using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Services;

public interface ITicketStatusService
{
    Task<ICollection<TicketStatusGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TicketStatusGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class TicketStatusService : ITicketStatusService
{
    private readonly IMapper _mapper;
    private readonly ITicketStatusQueryService _queries;

    public TicketStatusService(IMapper mapper, ITicketStatusQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task<ICollection<TicketStatusGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var statuses = await _queries.Get(cancellationToken);
        return _mapper.Map<ICollection<TicketStatusGetDto>>(statuses);
    }

    public async Task<TicketStatusGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var status = await _queries.Get(id, cancellationToken) ?? throw new InvalidIdException();
        return _mapper.Map<TicketStatusGetDto>(status);
    }
}
