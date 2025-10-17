using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class ExecutableMimetypeUpdateHandler : IExecutableMimetypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IExecutableMimetypeQueryService _executableMimetypeQueryService;
        private readonly IValidator<ExecutableMimetypeUpdateDto> _validator;

        public ExecutableMimetypeUpdateHandler(
            IMapper mapper,
            IExecutableMimetypeQueryService executableMimetypeQueryService,
            IValidator<ExecutableMimetypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _executableMimetypeQueryService = executableMimetypeQueryService;
            _executableMimetypeQueryService.NotNull(nameof(_executableMimetypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(ExecutableMimetypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var executableMimetype = await _executableMimetypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, executableMimetype);
        }
    }
}
