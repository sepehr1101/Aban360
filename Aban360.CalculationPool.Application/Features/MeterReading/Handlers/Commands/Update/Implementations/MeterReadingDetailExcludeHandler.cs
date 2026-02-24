using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Excluded.Implementations
{
    internal sealed class MeterReadingDetailExcludeHandler : AbstractBaseConnection, IMeterReadingDetailExcludeHandler
    {
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        public MeterReadingDetailExcludeHandler(
            IMeterReadingDetailQueryService meterReadingDetailService,
            IConfiguration configuration)
            :base(configuration)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));
        }

        public async Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken)
        {
            MeterReadingDetailExcludedDto readingCreateExcluded = new(id, appUser.UserId, DateTime.Now);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterReadingDetailCommandService meterReadingDetailCommandService = new(connection, transaction);
                    await meterReadingDetailCommandService.Exclude(readingCreateExcluded);
                    
                    transaction.Commit();
                }
            }
        }
    }
}
