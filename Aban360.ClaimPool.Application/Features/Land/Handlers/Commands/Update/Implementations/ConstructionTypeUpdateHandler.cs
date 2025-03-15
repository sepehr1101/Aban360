using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class ConstructionTypeUpdateHandler : IConstructionTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IConstructionTypeQueryService _queryService;
        public ConstructionTypeUpdateHandler(
            IMapper mapper,
            IConstructionTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(ConstructionTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            ConstructionType constructionType = await _queryService.Get(updateDto.Id);
            if (constructionType == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, constructionType);
        }
    }

}
