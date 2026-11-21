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
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
namespace BeforeServiceAuxiliary10.Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations{
    internal sealed class SmsTypeInsertHandler : AbstractBaseConnection, ISmsTypeInsertHandler
    {
        private readonly ISmsTypeService _smsTypeService;
        public SmsTypeInsertHandler(
            ISmsTypeService smsTypeService,
            IConfiguration configuration)
                : base(configuration)
        {
            _smsTypeService = smsTypeService;
            _smsTypeService.NotNull(nameof(smsTypeService));
        }

        public async Task Handle(string title, CancellationToken cancellationToken)
        {
            NumericDictionary? smsTypeInfo = await _smsTypeService.Get(title, false);
            if (smsTypeInfo is not null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidInsertDuplicateSmsType);
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
                    SmsTypeCommandService smsTypeCommandService = new(connection, transaction);
                    await smsTypeCommandService.Insert(title);

                    transaction.Commit();
                }
            }
        }
    }
}
