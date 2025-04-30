using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    internal sealed class TariffByDetailCreateHandler : ITariffByDetailCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffByDetailCommandService _tariffByDetailCommandService;
        public TariffByDetailCreateHandler(
            IMapper mapper,
            ITariffByDetailCommandService tariffByDetailCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffByDetailCommandService = tariffByDetailCommandService;
            _tariffByDetailCommandService.NotNull(nameof(tariffByDetailCommandService));
        }

        public async Task Handle(TariffByDetailCreateDto createDto, CancellationToken cancellationToken)
        {
            TariffByDetail tariffByDetail = _mapper.Map<TariffByDetail>(createDto);
            await _tariffByDetailCommandService.Add(tariffByDetail);
        }
    }
}
