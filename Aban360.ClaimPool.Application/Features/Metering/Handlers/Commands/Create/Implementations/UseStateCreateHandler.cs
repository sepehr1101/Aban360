using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    public class UseStateCreateHandler : IUseStateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUseStateCommandService _useStateCommandService;
        public UseStateCreateHandler(
            IMapper mapper,
            IUseStateCommandService useStateCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _useStateCommandService = useStateCommandService;
            _useStateCommandService.NotNull(nameof(useStateCommandService));
        }

        public async Task Handle(UseStateCreateDto createDto, CancellationToken cancellationToken)
        {
            var useState = _mapper.Map<UseState>(createDto);
            await _useStateCommandService.Add(useState);
        }
    }
}
