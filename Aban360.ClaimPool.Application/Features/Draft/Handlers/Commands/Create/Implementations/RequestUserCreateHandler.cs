using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestUserCreateHandler : IRequestUserCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestUserCommandService _requestUserCommandService;
        public RequestUserCreateHandler(
            IMapper mapper,
            IRequestUserCommandService requestUserCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestUserCommandService = requestUserCommandService;
            _requestUserCommandService.NotNull(nameof(_requestUserCommandService));
        }

        public async Task Handle(RequestUserCommandDto createDto, CancellationToken cancellationToken)
        {
            var requestUser = _mapper.Map<RequestUser>(createDto);
            await _requestUserCommandService.Add(requestUser);
        }
    }
}
