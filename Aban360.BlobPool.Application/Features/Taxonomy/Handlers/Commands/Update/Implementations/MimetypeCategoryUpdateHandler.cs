using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class MimetypeCategoryUpdateHandler : IMimetypeCategoryUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMimetypeCategoryQueryService _mimetypeCategoryQueryService;
        private readonly IValidator<MimetypeCategoryUpdateDto> _validator;

        public MimetypeCategoryUpdateHandler(
            IMapper mapper,
            IMimetypeCategoryQueryService mimetypeCategoryQueryService,
            IValidator<MimetypeCategoryUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _mimetypeCategoryQueryService = mimetypeCategoryQueryService;
            _mimetypeCategoryQueryService.NotNull(nameof(_mimetypeCategoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MimetypeCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var mimetypeCategory = await _mimetypeCategoryQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, mimetypeCategory);
        }
    }
}
