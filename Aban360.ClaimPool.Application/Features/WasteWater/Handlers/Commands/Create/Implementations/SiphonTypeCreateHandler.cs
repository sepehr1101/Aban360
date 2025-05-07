using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using FluentValidation;
using System.Threading;
using Aban360.Common.Exceptions;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    internal sealed class SiphonTypeCreateHandler : ISiphonTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonTypeCommandService _commandService;
        private readonly IValidator<SiphonTypeCreateDto> _validator;
        public SiphonTypeCreateHandler(
            IMapper mapper,
            ISiphonTypeCommandService commandService,
            IValidator<SiphonTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SiphonTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            SiphonType SiphonType = _mapper.Map<SiphonType>(createDto);
            await _commandService.Add(SiphonType);
        }
    }
}
