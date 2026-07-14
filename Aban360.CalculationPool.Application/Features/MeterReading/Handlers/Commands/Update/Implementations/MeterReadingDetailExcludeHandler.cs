using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
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
            : base(configuration)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));
        }

        public async Task Handle(MeterReadingDetailExcludeInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            string causeTitle = inputDto.CauseId switch
            {
                ExcludedCauseEnum.PriGTCurrent => ReportLiterals.PriGTCurrent,
                ExcludedCauseEnum.NeedEvaluate => ReportLiterals.NeedEvaluate,
                ExcludedCauseEnum.Error => ReportLiterals.Error,
                _ => string.Empty,
            };
            MeterReadingDetailExcludedDto readingCreateExcluded = new(inputDto.Id, appUser.UserId, DateTime.Now, inputDto.CauseId, causeTitle);
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
