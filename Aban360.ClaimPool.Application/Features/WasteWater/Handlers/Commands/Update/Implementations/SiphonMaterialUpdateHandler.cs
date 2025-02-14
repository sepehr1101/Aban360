using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    public class SiphonMaterialUpdateHandler : ISiphonMaterialUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonMaterialQueryService _queryService;
        public SiphonMaterialUpdateHandler(
            IMapper mapper,
            ISiphonMaterialQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(SiphonMaterialUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var SiphonMaterial = await _queryService.Get(updateDto.Id);
            if (SiphonMaterial == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, SiphonMaterial);
        }
    }
}
