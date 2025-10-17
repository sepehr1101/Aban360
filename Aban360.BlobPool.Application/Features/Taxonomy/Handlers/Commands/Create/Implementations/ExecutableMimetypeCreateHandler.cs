using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Implementations
{
    internal sealed class ExecutableMimetypeCreateHandler : IExecutableMimetypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IExecutableMimetypeCommandService _executableMimetypeCommandService;
        private readonly IValidator<ExecutableMimetypeCreateDto> _validator;

        public ExecutableMimetypeCreateHandler(
            IMapper mapper,
            IExecutableMimetypeCommandService executableMimetypeCommandService,
            IValidator<ExecutableMimetypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _executableMimetypeCommandService = executableMimetypeCommandService;
            _executableMimetypeCommandService.NotNull(nameof(_executableMimetypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(ExecutableMimetypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var ExecutableMimetype = _mapper.Map<ExecutableMimetype>(createDto);
            await _executableMimetypeCommandService.Add(ExecutableMimetype);
        }
    }
}
