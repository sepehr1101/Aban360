using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class OtherExpensesItemsRemoveHandler : AbstractBaseConnection, IOtherExpensesItemsRemoveHandler
    {
        private readonly IOtherExpensesItemsQueryService _otherExpensesItemsQueryService;
        public OtherExpensesItemsRemoveHandler(
            IOtherExpensesItemsQueryService otherExpensesItemsQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _otherExpensesItemsQueryService = otherExpensesItemsQueryService;
            _otherExpensesItemsQueryService.NotNull(nameof(otherExpensesItemsQueryService));
        }

        public async Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken)
        {
            OtherExpensesItemsGetDto itemInfo = await _otherExpensesItemsQueryService.Get(id);
            OtherExpensesItemsRemoveDto RemoveDto = GetRemoveDto(itemInfo, appUser);
            await ExceSql(RemoveDto);
        }
        private async Task ExceSql(OtherExpensesItemsRemoveDto otherExpensesItmesRemoveDto)
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
                    await otherExpensesItmesCommandService.Remove(otherExpensesItmesRemoveDto);

                    transaction.Commit();
                }
            }
        }
        private OtherExpensesItemsRemoveDto GetRemoveDto(OtherExpensesItemsGetDto itemInfo, IAppUser appUser)
        {
            return new OtherExpensesItemsRemoveDto()
            {
                Id = itemInfo.Id,
                RemoveBy = appUser.UserId,
                RemoveDateTime = DateTime.Now,
            };
        }
    }
}
