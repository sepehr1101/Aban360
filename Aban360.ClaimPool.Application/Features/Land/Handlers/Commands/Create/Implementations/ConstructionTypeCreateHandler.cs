using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class ConstructionTypeCreateHandler : IConstructionTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IConstructionTypeCommandService _commandService;
        private readonly IValidator<ConstructionTypeCreateDto> _validator;

        public ConstructionTypeCreateHandler(
            IMapper mapper,
            IConstructionTypeCommandService commandService,
            IValidator<ConstructionTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(_commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(ConstructionTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ConstructionType constructionType = _mapper.Map<ConstructionType>(createDto);
            await _commandService.Add(constructionType);
        }
    }

}
