using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    internal sealed class ClientCommentInsertHandler : IClientCommentInsertHandler
    {
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IClientCommentCommandService _clientCommentCommandService;
        private readonly IValidator<CustomerCommentInputDto> _validator;
        public ClientCommentInsertHandler(
            ICommonMemberQueryService commonMemberQueryService,
            IClientCommentCommandService userCommentCommandService,
            IValidator<CustomerCommentInputDto> validator)
        {
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _clientCommentCommandService = userCommentCommandService;
            _clientCommentCommandService.NotNull(nameof(userCommentCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(CustomerCommentInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);

            ClientCommentInsertDto insertDto = new()
            {
                BillId=inputDto.BillId,
                Comment=inputDto.Comment,
                UserDisplayName=appUser.Username,
                UserId=appUser.UserId,
            };
            await _clientCommentCommandService.Insert(insertDto);
        }
        private async Task Validation(CustomerCommentInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputDtoValidation(inputDto, cancellationToken);
            await BillIdValidation(inputDto.BillId);
        }
        private async Task InputDtoValidation(CustomerCommentInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private async Task BillIdValidation(string billId)
        {
            ZoneIdAndCustomerNumber result = await _commonMemberQueryService.Get(billId);
            if (result == null || result.ZoneId <= 0)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }
        }
    }
}
