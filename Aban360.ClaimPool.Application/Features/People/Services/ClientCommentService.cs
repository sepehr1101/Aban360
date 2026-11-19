using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Services;
public interface IClientCommentService
{
    Task InsertAsync(CustomerCommentInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<CustomerCommentGetDto>> GetByBillIdAsync(string billId, CancellationToken cancellationToken);
}

internal sealed class ClientCommentService : IClientCommentService
{
    private readonly ICommonMemberQueryService _commonMemberQueryService;
    private readonly IClientCommentCommandService _clientCommentCommandService;
    private readonly IValidator<CustomerCommentInputDto> _validator;
    private readonly IClientCommentQueryService _clientCommentQueryService;
    public ClientCommentService(ICommonMemberQueryService commonMemberQueryService, IClientCommentCommandService userCommentCommandService, IValidator<CustomerCommentInputDto> validator, IClientCommentQueryService userCommentQueryService)
    {
        _commonMemberQueryService = commonMemberQueryService;
        _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        _clientCommentCommandService = userCommentCommandService;
        _clientCommentCommandService.NotNull(nameof(userCommentCommandService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _clientCommentQueryService = userCommentQueryService;
        _clientCommentQueryService.NotNull(nameof(userCommentQueryService));
    }

    public async Task InsertAsync(CustomerCommentInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await InsertValidation(inputDto, cancellationToken);
        ClientCommentInsertDto insertDto = new()
        {
            BillId = inputDto.BillId,
            Comment = inputDto.Comment,
            UserDisplayName = appUser.Username,
            UserId = appUser.UserId,
        };
        await _clientCommentCommandService.Insert(insertDto);
    }

    private async Task InsertValidation(CustomerCommentInputDto inputDto, CancellationToken cancellationToken)
    {
        await InsertInputDtoValidation(inputDto, cancellationToken);
        await InsertBillIdValidation(inputDto.BillId);
    }

    private async Task InsertInputDtoValidation(CustomerCommentInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task InsertBillIdValidation(string billId)
    {
        ZoneIdAndCustomerNumber result = await _commonMemberQueryService.Get(billId);
        if (result == null || result.ZoneId <= 0)
        {
            throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
        }
    }

    public async Task<IEnumerable<CustomerCommentGetDto>> GetByBillIdAsync(string billId, CancellationToken cancellationToken)
    {
        IEnumerable<CustomerCommentGetDto> result = await _clientCommentQueryService.Get(billId);
        result.ForEach(u => u.InsertDateJalali = u.InsertDateTime.ToShortPersianDateString());
        return result;
    }
}
