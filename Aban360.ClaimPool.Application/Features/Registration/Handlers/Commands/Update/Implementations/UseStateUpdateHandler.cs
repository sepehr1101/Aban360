using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Update.Implementations
{
    public class UseStateUpdateHandler : IUseStateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUseStateQueryService _useStateQueryService;
        public UseStateUpdateHandler(
            IMapper mapper,
            IUseStateQueryService useStateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _useStateQueryService = useStateQueryService;
            _useStateQueryService.NotNull(nameof(useStateQueryService));
        }

        public async Task Handle(UseStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var useState = await _useStateQueryService.Get(updateDto.Id);
            if (useState == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, useState);
        }
    }
}
