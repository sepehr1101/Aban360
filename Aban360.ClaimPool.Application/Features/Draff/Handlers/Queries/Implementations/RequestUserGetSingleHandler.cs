using Aban360.ClaimPool.Application.Features.Draff.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draff.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Draff.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draff.Handlers.Queries.Implementations
{
    internal sealed class RequestUserGetSingleHandler : IRequestUserGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestUserQueryService _requestUserQueryService;
        public RequestUserGetSingleHandler(
            IMapper mapper,
            IRequestUserQueryService requestUserQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestUserQueryService = requestUserQueryService;
            _requestUserQueryService.NotNull(nameof(_requestUserQueryService));
        }

        public async Task<RequestUserQueryDto> Handle(short id, CancellationToken cancellationToken)
        {
            var requestUser = await _requestUserQueryService.Get(id);
            if (requestUser == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<RequestUserQueryDto>(requestUser);
        }
    }
}
