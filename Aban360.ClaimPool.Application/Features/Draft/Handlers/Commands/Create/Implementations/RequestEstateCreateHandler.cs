using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestEstateCreateHandler : IRequestEstateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestEstateCommandService _requestEstateCommandService;
        public RequestEstateCreateHandler(
            IMapper mapper,
            IRequestEstateCommandService requestEstateCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestEstateCommandService = requestEstateCommandService;
            _requestEstateCommandService.NotNull(nameof(_requestEstateCommandService));
        }

        public async Task Handle(EstateRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestEstate = _mapper.Map<RequestEstate>(createDto);
            await _requestEstateCommandService.Add(requestEstate);
        }
    }
}
