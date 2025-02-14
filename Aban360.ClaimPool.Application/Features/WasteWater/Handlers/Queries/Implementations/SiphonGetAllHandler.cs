using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    public class SiphonGetAllHandler : ISiphonGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonQueryService _queryService;
        public SiphonGetAllHandler(
            IMapper mapper,
            ISiphonQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<SiphonGetDto>> Handle(CancellationToken cancellationToken)
        {
            var siphon = await _queryService.Get();
            if (siphon == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<SiphonGetDto>>(siphon);
        }
    }


}
