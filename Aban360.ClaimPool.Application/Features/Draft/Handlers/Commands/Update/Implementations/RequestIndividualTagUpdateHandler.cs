using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestIndividualTagUpdateHandler : IRequestIndividualTagUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualTagQueryService _requestIndividualTagQueryService;
        public RequestIndividualTagUpdateHandler(
            IMapper mapper,
            IRequestIndividualTagQueryService requestIndividualTagQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualTagQueryService = requestIndividualTagQueryService;
            _requestIndividualTagQueryService.NotNull(nameof(_requestIndividualTagQueryService));
        }

        public async Task Handle(IndividualTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestIndividualTag = await _requestIndividualTagQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestIndividualTag);
        }
    }
}
