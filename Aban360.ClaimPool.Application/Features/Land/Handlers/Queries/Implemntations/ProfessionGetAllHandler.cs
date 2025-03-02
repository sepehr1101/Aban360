using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class ProfessionGetAllHandler : IProfessionGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IProfessionQueryService _queryService;
        public ProfessionGetAllHandler(
            IMapper mapper,
            IProfessionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<ProfessionGetDto>> Handle(CancellationToken cancellationToken)
        {
            var profession = await _queryService.Get();
            if (profession == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<ProfessionGetDto>>(profession);
        }
    }
}
