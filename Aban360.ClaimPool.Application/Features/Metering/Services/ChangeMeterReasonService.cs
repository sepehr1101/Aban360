using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Services;

public interface IChangeMeterReasonService
{
    Task<ICollection<ChangeMeterReasonGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ChangeMeterReasonGetDto> GetSingleAsync(ChangeMeterReasonEnum id, CancellationToken cancellationToken);
}

internal sealed class ChangeMeterReasonService : IChangeMeterReasonService
{
    private readonly IMapper _mapper;
    private readonly IChangeMeterReasonQueryService _queries;

    public ChangeMeterReasonService(IMapper mapper, IChangeMeterReasonQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task<ICollection<ChangeMeterReasonGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<ChangeMeterReason>  changeMeterReason = await _queries.Get();
        return _mapper.Map<ICollection<ChangeMeterReasonGetDto>>(changeMeterReason);
    }

    public async Task<ChangeMeterReasonGetDto> GetSingleAsync(ChangeMeterReasonEnum id, CancellationToken cancellationToken)
    {
        ChangeMeterReason changeMeterReason = await _queries.Get(id);
        return _mapper.Map<ChangeMeterReasonGetDto>(changeMeterReason);
    }
}
