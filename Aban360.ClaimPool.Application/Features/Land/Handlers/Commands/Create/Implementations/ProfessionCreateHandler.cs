using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class ProfessionCreateHandler : IProfessionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IProfessionCommandService _professionCommandService;
        public ProfessionCreateHandler(
            IMapper mapper,
            IProfessionCommandService professionCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _professionCommandService = professionCommandService;
            _professionCommandService.NotNull(nameof(_professionCommandService));
        }

        public async Task Handle(ProfessionCreateDto createDto, CancellationToken cancellationToken)
        {
            Profession profession = _mapper.Map<Profession>(createDto);
            await _professionCommandService.Add(profession);
        }
    }
}
