using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Implementations
{
    internal sealed class TariffByDetailUpdateHandler : ITariffByDetailUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffByDetailQueryService _tariffByDetailQueryService;
        public TariffByDetailUpdateHandler(
            IMapper mapper,
            ITariffByDetailQueryService tariffByDetailQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _tariffByDetailQueryService = tariffByDetailQueryService;
            _tariffByDetailQueryService.NotNull(nameof(_tariffByDetailQueryService));
        }

        public async Task Handle(TariffByDetailUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var tariffByDetail = await _tariffByDetailQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, tariffByDetail);
        }
    }
}
