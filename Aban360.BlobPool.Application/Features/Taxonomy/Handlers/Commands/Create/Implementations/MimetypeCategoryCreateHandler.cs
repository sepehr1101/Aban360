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
    internal sealed class MimetypeCategoryCreateHandler : IMimetypeCategoryCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMimetypeCategoryCommandService _mimetypeCategoryCommandService;
        private readonly IValidator<MimetypeCategoryCreateDto> _validator;

        public MimetypeCategoryCreateHandler(
            IMapper mapper,
            IMimetypeCategoryCommandService mimetypeCategoryCommandService,
            IValidator<MimetypeCategoryCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _mimetypeCategoryCommandService = mimetypeCategoryCommandService;
            _mimetypeCategoryCommandService.NotNull(nameof(_mimetypeCategoryCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MimetypeCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var mimetypeCategory = _mapper.Map<MimetypeCategory>(createDto);
            await _mimetypeCategoryCommandService.Add(mimetypeCategory);
        }
    }
}
