using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    internal sealed class SiphonUpdateHandler : ISiphonUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonQueryService _queryService;
        public SiphonUpdateHandler(
            IMapper mapper,
            ISiphonQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(SiphonUpdateDto updateDto, CancellationToken cancellationToken)
        {
            Siphon siphon = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, siphon);
        }
    }


}
