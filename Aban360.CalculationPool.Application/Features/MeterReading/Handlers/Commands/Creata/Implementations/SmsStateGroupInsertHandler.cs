using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class SmsStateGroupInsertHandler : AbstractBaseConnection, ISmsStateGroupInsertHandler
    {
        private readonly ISmsStateGroupService _SmsStateGroupService;
        public SmsStateGroupInsertHandler(
            ISmsStateGroupService SmsStateGroupService,
            IConfiguration configuration)
                : base(configuration)
        {
            _SmsStateGroupService = SmsStateGroupService;
            _SmsStateGroupService.NotNull(nameof(SmsStateGroupService));
        }

        public async Task Handle(string title, CancellationToken cancellationToken)
        {
            NumericDictionary? SmsStateGroupInfo = await _SmsStateGroupService.Get(title, false);
            if (SmsStateGroupInfo is not null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidInsertDuplicateSmsStateGroup);
            }
            await ExecSql(title);
        }
        private async Task ExecSql(string title)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    SmsStateGroupCommandService SmsStateGroupCommandService = new(connection, transaction);
                    await SmsStateGroupCommandService.Insert(title);

                    transaction.Commit();
                }
            }
        }
    }
}
