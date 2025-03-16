using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class GetewayCreateHandler : IGetewayCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IGetewayCommandService _getewayCommandService;
        public GetewayCreateHandler(
            IMapper mapper,
            IGetewayCommandService getewayCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _getewayCommandService = getewayCommandService;
            _getewayCommandService.NotNull(nameof(_getewayCommandService));
        }

        public async Task Handle(GetewayCreateDto createDto, CancellationToken cancellationToken)
        {
            Geteway geteway = _mapper.Map<Geteway>(createDto);
            if (geteway == null)
            {
                throw new InvalidDataException();
            }
            await _getewayCommandService.Add(geteway);
        }
    }
}