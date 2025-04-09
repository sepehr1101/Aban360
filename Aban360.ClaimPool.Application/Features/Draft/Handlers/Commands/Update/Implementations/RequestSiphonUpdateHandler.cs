using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestSiphonUpdateHandler : IRequestSiphonUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestSiphonQueryService _requestSiphonQueryService;
        public RequestSiphonUpdateHandler(
            IMapper mapper,
            IRequestSiphonQueryService requestSiphonQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestSiphonQueryService = requestSiphonQueryService;
            _requestSiphonQueryService.NotNull(nameof(_requestSiphonQueryService));
        }

        public async Task Handle(SiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestSiphon = await _requestSiphonQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestSiphon);
        }
    }
}
