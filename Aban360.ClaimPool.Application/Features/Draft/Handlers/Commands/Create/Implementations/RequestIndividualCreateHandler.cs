using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestIndividualCreateHandler : IRequestIndividualCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualCommandService _requestIndividualCommandService;
        public RequestIndividualCreateHandler(
            IMapper mapper,
            IRequestIndividualCommandService requestIndividualCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualCommandService = requestIndividualCommandService;
            _requestIndividualCommandService.NotNull(nameof(_requestIndividualCommandService));
        }

        public async Task Handle(IndividualRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestIndividual = _mapper.Map<RequestIndividual>(createDto);
            await _requestIndividualCommandService.Add(requestIndividual);
        }
    }
}
