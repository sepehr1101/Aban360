using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class CordinalDirectionUpdateHandler : ICordinalDirectionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICordinalDirectionQueryService _cordinalDirectionQeuryService;
        public CordinalDirectionUpdateHandler(
            IMapper mapper,
            ICordinalDirectionQueryService cordinalDirectionQeuryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _cordinalDirectionQeuryService = cordinalDirectionQeuryService;
            _cordinalDirectionQeuryService.NotNull(nameof(cordinalDirectionQeuryService));
        }

        public async Task Handle(CordinalDirectionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            CordinalDirection cordinalDirection = await _cordinalDirectionQeuryService.Get(updateDto.Id);
            _mapper.Map(updateDto, cordinalDirection);
        }
    }
}
