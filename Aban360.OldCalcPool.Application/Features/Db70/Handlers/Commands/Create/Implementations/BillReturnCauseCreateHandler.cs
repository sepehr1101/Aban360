using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Implementations
{
    internal sealed class BillReturnCauseCreateHandler : IBillReturnCauseCreateHandler
    {
        private readonly IBillReturnCauseCommandService _billReturnCauseCommandService;
        private readonly IValidator<BillReturnCauseCreateDto> _validator;

        public BillReturnCauseCreateHandler(
            IBillReturnCauseCommandService billReturnCauseCommandService,
            IValidator<BillReturnCauseCreateDto> validator)
        {
            _billReturnCauseCommandService = billReturnCauseCommandService;
            _billReturnCauseCommandService.NotNull(nameof(billReturnCauseCommandService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(BillReturnCauseCreateDto createDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            createDto.RegisterDateTime = DateTime.Now;
            createDto.RegisterByUserId = appUser.UserId;
            await _billReturnCauseCommandService.Create(createDto);
        }
    }
}
