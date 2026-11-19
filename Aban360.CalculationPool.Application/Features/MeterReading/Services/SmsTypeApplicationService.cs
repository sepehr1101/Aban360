using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Services;

public interface ISmsTypeApplicationService
{
    Task CreateAsync(string title, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class SmsTypeApplicationService : AbstractBaseConnection, ISmsTypeApplicationService
{
    private readonly ISmsTypeService _smsTypeService;
    public SmsTypeApplicationService(
        ISmsTypeService smsTypeService,
        IConfiguration configuration) : base(configuration)
    {
        _smsTypeService = smsTypeService;
        _smsTypeService.NotNull(nameof(smsTypeService));
    }

    public async Task CreateAsync(string title, CancellationToken cancellationToken)
    {
        NumericDictionary? smsTypeInfo = await _smsTypeService.Get(title, false);
        if (smsTypeInfo is not null)
        {
            throw new ReadingException(ExceptionLiterals.InvalidInsertDuplicateSmsType);
        }

        await CreateExecSql(title);
    }

    private async Task CreateExecSql(string title)
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

    public async Task<IEnumerable<NumericDictionary>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NumericDictionary> SmsType = await _smsTypeService.Get();
        return SmsType;
    }
}
