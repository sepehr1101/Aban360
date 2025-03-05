using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Implementations
{
    public class TariffConstantUpdateHandler : ITariffConstantUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffConstantQueryService _tariffConstantQueryService;
        public TariffConstantUpdateHandler(
            IMapper mapper,
            ITariffConstantQueryService tariffConstantQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffConstantQueryService = tariffConstantQueryService;
            _tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));
        }

        public async Task Handle(TariffConstantUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var tariffConstant = await _tariffConstantQueryService.Get(updateDto.Id);
            if (tariffConstant == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, tariffConstant);
        }
    }
}
