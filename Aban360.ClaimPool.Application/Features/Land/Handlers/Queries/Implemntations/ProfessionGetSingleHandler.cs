using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ProfessionGetSingleHandler : IProfessionGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IProfessionQueryService _queryService;
        public ProfessionGetSingleHandler(
            IMapper mapper,
            IProfessionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ProfessionGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            Profession profession = await _queryService.Get(id);
            if (profession == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ProfessionGetDto>(profession);
        }
    }
}
