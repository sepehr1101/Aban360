using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestFlatCreateHandler : IRequestFlatCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestFlatCommandService _requestFlatCommandService;
        public RequestFlatCreateHandler(
            IMapper mapper,
            IRequestFlatCommandService requestFlatCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestFlatCommandService = requestFlatCommandService;
            _requestFlatCommandService.NotNull(nameof(_requestFlatCommandService));
        }

        public async Task Handle(FlatRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestFlat = _mapper.Map<RequestFlat>(createDto);
            await _requestFlatCommandService.Add(requestFlat);
        }
    }
}
