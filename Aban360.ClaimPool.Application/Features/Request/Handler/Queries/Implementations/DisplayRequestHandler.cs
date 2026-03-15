using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class DisplayRequestHandler : IDisplayRequestHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IValidator<ZoneIdAndTrackNumber> _validator;
        public DisplayRequestHandler(
            IMoshtrakQueryService moshtrakQueryService,
            IValidator<ZoneIdAndTrackNumber> validator)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<MoshtrakOutputDto> Handle(ZoneIdAndTrackNumber inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);

            MoshtrakGetDto moshtrackSearch = new(inputDto.ZoneId, null, null, inputDto.TrackNumber);
            IEnumerable<MoshtrakOutputDto> moshtrakInfo = await _moshtrakQueryService.Get(moshtrackSearch, MoshtrakSearchTypeEnum.ByTrackNumber);
            return moshtrakInfo.FirstOrDefault();
        }
        private async Task Validation(ZoneIdAndTrackNumber inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
