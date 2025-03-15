using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class CordinalDirectionGetAllHandler : ICordinalDirectionGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ICordinalDirectionQueryService _cordinalDirectionQueryService;
        public CordinalDirectionGetAllHandler(
            IMapper mapper,
            ICordinalDirectionQueryService cordinalDirectionQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _cordinalDirectionQueryService = cordinalDirectionQueryService;
            _cordinalDirectionQueryService.NotNull(nameof(cordinalDirectionQueryService));
        }

        public async Task<ICollection<CordinalDirectionGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<CordinalDirection> cordinalDirection = await _cordinalDirectionQueryService.Get();
            return _mapper.Map<ICollection<CordinalDirectionGetDto>>(cordinalDirection);
        }
    }
}
