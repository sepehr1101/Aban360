using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class ConstructionTypeGetSingleHandler : IConstructionTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IConstructionTypeQueryService _queryService;
        public ConstructionTypeGetSingleHandler(
            IMapper mapper,
            IConstructionTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ConstructionTypeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var constructionType = await _queryService.Get(id);
            if (constructionType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ConstructionTypeGetDto>(constructionType);
        }
    }

}
