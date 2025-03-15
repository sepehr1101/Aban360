using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class EstateBoundTypeGetSingleHandler : IEstateBoundTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateBoundTypeQueryService _queryService;
        public EstateBoundTypeGetSingleHandler(
            IMapper mapper,
           IEstateBoundTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<EstateBoundTypeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            EstateBoundType estateBoundType = await _queryService.Get(id);
            if (estateBoundType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<EstateBoundTypeGetDto>(estateBoundType);
        }
    }
}
