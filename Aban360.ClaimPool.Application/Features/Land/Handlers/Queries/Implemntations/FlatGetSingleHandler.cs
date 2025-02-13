using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class FlatGetSingleHandler : IFlatGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IFlatQueryService _queryService;
        public FlatGetSingleHandler(
            IMapper mapper,
            IFlatQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<FlatGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var flat = await _queryService.Get(id);
            if (flat == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<FlatGetDto>(flat);
        }
    }
}
