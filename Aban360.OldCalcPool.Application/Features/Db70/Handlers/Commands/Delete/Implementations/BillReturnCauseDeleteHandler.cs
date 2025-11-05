using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Implementations
{
    internal sealed class BillReturnCauseDeleteHandler : IBillReturnCauseDeleteHandler
    {
        private readonly IBillReturnCauseCommandService _billReturnCauseCommandService;
        private readonly IValidator<BillReturnCauseDeleteDto> _validator;

        public BillReturnCauseDeleteHandler(
            IBillReturnCauseCommandService billReturnCauseCommandService,
            IValidator<BillReturnCauseDeleteDto> validator)
        {
            _billReturnCauseCommandService = billReturnCauseCommandService;
            _billReturnCauseCommandService.NotNull(nameof(billReturnCauseCommandService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(BillReturnCauseDeleteDto deleteDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(deleteDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            deleteDto.RemoveDateTime = DateTime.Now;
            deleteDto.RemoveByUserId = appUser.UserId;
            await _billReturnCauseCommandService.Delete(deleteDto);
        }
    }
}
