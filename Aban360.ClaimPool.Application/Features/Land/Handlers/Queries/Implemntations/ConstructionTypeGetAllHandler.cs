using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ConstructionTypeGetAllHandler : IConstructionTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IConstructionTypeQueryService _queryService;
        public ConstructionTypeGetAllHandler(
            IMapper mapper,
            IConstructionTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<ConstructionTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<ConstructionType> constructionType = await _queryService.Get();
            if (constructionType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<ConstructionTypeGetDto>>(constructionType);
        }
    }

}
