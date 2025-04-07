using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestEstateUpdateHandler : IRequestEstateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestEstateQueryService _requestEstateQueryService;
        public RequestEstateUpdateHandler(
            IMapper mapper,
            IRequestEstateQueryService requestEstateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestEstateQueryService = requestEstateQueryService;
            _requestEstateQueryService.NotNull(nameof(_requestEstateQueryService));
        }

        public async Task Handle(EstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestEstate = await _requestEstateQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestEstate);
        }
    }
}
