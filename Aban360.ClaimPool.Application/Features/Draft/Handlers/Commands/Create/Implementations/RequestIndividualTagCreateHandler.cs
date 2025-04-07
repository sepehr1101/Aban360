using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestIndividualTagCreateHandler : IRequestIndividualTagCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualTagCommandService _requestIndividualTagCommandService;
        public RequestIndividualTagCreateHandler(
            IMapper mapper,
            IRequestIndividualTagCommandService requestIndividualTagCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualTagCommandService = requestIndividualTagCommandService;
            _requestIndividualTagCommandService.NotNull(nameof(_requestIndividualTagCommandService));
        }

        public async Task Handle(IndividualTagRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestIndividualTag = _mapper.Map<RequestIndividualTag>(createDto);
            await _requestIndividualTagCommandService.Add(requestIndividualTag);
        }
    }
}
