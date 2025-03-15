using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class EstateGetSingleHandler : IEstateGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateQueryService _queryService;
        public EstateGetSingleHandler(
            IMapper mapper,
           IEstateQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<EstateGetDto> Handle(int id,CancellationToken cancellationToken)
        {
            Estate estate = await _queryService.Get(id);
            if (estate == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<EstateGetDto>(estate);
        }
    }
}
