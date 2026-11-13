using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestUserService
{
    Task<RequestUserQueryDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<RequestUserQueryDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class RequestUserService : IRequestUserService
{
    private readonly IMapper _mapper;
    private readonly IRequestUserQueryService _queries;

    public RequestUserService(IMapper mapper, IRequestUserQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task<RequestUserQueryDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var requestUser = await _queries.Get(id);
        if (requestUser == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<RequestUserQueryDto>(requestUser);
    }

    public async Task<ICollection<RequestUserQueryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var requestUser = await _queries.Get();
        if (requestUser == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<RequestUserQueryDto>>(requestUser);
    }
}
