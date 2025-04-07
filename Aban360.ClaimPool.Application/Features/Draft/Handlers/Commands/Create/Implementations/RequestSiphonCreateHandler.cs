using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestSiphonCreateHandler : IRequestSiphonCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestSiphonCommandService _requestSiphonCommandService;
        public RequestSiphonCreateHandler(
            IMapper mapper,
            IRequestSiphonCommandService requestSiphonCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestSiphonCommandService = requestSiphonCommandService;
            _requestSiphonCommandService.NotNull(nameof(_requestSiphonCommandService));
        }

        public async Task Handle(SiphonRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestSiphon = _mapper.Map<RequestSiphon>(createDto);
            await _requestSiphonCommandService.Add(requestSiphon);
        }
    }
}
