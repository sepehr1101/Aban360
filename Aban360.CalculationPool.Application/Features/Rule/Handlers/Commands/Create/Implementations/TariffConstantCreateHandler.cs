using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    public class TariffConstantCreateHandler : ITariffConstantCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffConstantCommandService _tariffConstantCommandService;
        public TariffConstantCreateHandler(
            IMapper mapper,
            ITariffConstantCommandService tariffConstantCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffConstantCommandService = tariffConstantCommandService;
            _tariffConstantCommandService.NotNull(nameof(tariffConstantCommandService));
        }

        public async Task Handle(TariffConstantCreateDto createDto, CancellationToken cancellationToken)
        {
            var tariffConstant = _mapper.Map<TariffConstant>(createDto);
            if (tariffConstant == null)
            {
                throw new InvalidDataException();
            }
            await _tariffConstantCommandService.Add(tariffConstant);
        }
    }
}
