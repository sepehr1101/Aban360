using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestTrackingCreateHandler : IRequestTrackingCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestTrackingCommandService _requestTrackingCommandService;
        public RequestTrackingCreateHandler(
            IMapper mapper,
            IRequestTrackingCommandService requestTrackingCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestTrackingCommandService = requestTrackingCommandService;
            _requestTrackingCommandService.NotNull(nameof(_requestTrackingCommandService));
        }

        public async Task Handle(IAppUser currentUser,RequestTrackingCreateDto createDto, CancellationToken cancellationToken)
        {
            RequestTracking requestTracking = _mapper.Map<RequestTracking>(createDto);
            requestTracking.UserId=currentUser.UserId;
            requestTracking.ValidFrom = DateTime.Now;
            requestTracking.Hash = "--";
            requestTracking.InsertLogInfo = "--";

            await _requestTrackingCommandService.Add(requestTracking);
        }
    }
}
