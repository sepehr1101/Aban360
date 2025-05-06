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
    internal sealed class OfficialHolidayCreateHandler : IOfficialHolidayCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfficialHolidayCommandService _officialHolidayCommandService;
        private readonly IValidator<OfficialHolidayCreateDto> _validator;

        public OfficialHolidayCreateHandler(
            IMapper mapper,
            IOfficialHolidayCommandService officialHolidayCommandService,
            IValidator<OfficialHolidayCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _officialHolidayCommandService = officialHolidayCommandService ?? throw new ArgumentNullException(nameof(officialHolidayCommandService));
            _officialHolidayCommandService.NotNull(nameof(_officialHolidayCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(OfficialHolidayCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var officialHoliday = _mapper.Map<OfficialHoliday>(createDto);
            await _officialHolidayCommandService.Add(officialHoliday);
        }
    }
}
