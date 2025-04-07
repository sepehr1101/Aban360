using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestFlatUpdateHandler : IRequestFlatUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestFlatQueryService _requestFlatQueryService;
        public RequestFlatUpdateHandler(
            IMapper mapper,
            IRequestFlatQueryService requestFlatQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestFlatQueryService = requestFlatQueryService;
            _requestFlatQueryService.NotNull(nameof(_requestFlatQueryService));
        }

        public async Task Handle(FlatRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestFlat = await _requestFlatQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestFlat);
        }
    }
}
