using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class OtherExpensesItemsInsertHandler : AbstractBaseConnection, IOtherExpensesItemsInsertHandler
    {
        private readonly IValidator<OtherExpensesItemsInsertInputDto> _validator;
        public OtherExpensesItemsInsertHandler(
            IValidator<OtherExpensesItemsInsertInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(OtherExpensesItemsInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            OtherExpensesItemsInsertDto insertDto = GetInsertDto(inputDto, appUser);
            await ExceSql(insertDto);
        }
        private async Task ExceSql(OtherExpensesItemsInsertDto otherExpensesItmesInsertDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    OtherExpensesItemsCommandService otherExpensesItmesCommandService = new(connection, transaction);
                    await otherExpensesItmesCommandService.Insert(otherExpensesItmesInsertDto);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(OtherExpensesItemsInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private OtherExpensesItemsInsertDto GetInsertDto(OtherExpensesItemsInsertInputDto inputDto, IAppUser appUser)
        {
            return new OtherExpensesItemsInsertDto()
            {
                ItemId = inputDto.ItemId,
                ItemTitle = string.Empty,//todo: find itemTitle
                Amount = inputDto.Amount,
                InsertBy = appUser.UserId,
                InsertDateTime = DateTime.Now,
            };
        }
    }
}
