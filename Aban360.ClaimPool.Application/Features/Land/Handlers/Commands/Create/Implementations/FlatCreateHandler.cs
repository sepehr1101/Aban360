using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class FlatCreateHandler : IFlatCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IFlatCommandService _commandService;
        private readonly IValidator<FlatCreateDto> _flatValidator;
        public FlatCreateHandler(
            IMapper mapper,
            IFlatCommandService commandService,
            IValidator<FlatCreateDto> estateValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _flatValidator = estateValidator;
            _flatValidator.NotNull(nameof(estateValidator));
        }

        public async Task Handle(FlatCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _flatValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }

            Flat flat = _mapper.Map<Flat>(createDto);
            await _commandService.Add(flat);
        }
    }
}
