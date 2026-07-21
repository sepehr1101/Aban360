using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dms.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Dms;
using Aban360.ReportPool.Persistence.Features.Dms.Commands;
using Aban360.ReportPool.Persistence.Features.Dms.Queries;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Dms.Commands.Implementations
{
    internal sealed class ClientDiscountInsertHandler : AbstractBaseConnection, IClientDiscountInsertHandler
    {
        private readonly IRequestDiscountService _clientDiscountService;
        public ClientDiscountInsertHandler(
            IRequestDiscountService clientDiscountService,
            IConfiguration configuration)
            : base(configuration)
        {
            _clientDiscountService = clientDiscountService;
            _clientDiscountService.NotNull(nameof(clientDiscountService));
        }

        public async Task Handle(ClientDiscountInsertDto input, CancellationToken cancellationToken)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                bool alreadyExists = await _clientDiscountService.Exists(input.CodeMeli.Trim());
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    RequestDiscountCommandService clientDiscountCommandService = new(connection, transaction);
                    if (alreadyExists)
                    {
                        await clientDiscountCommandService.UpdateByCodeMeli(input);
                    }
                    else
                    {
                        await clientDiscountCommandService.Insert(input);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
