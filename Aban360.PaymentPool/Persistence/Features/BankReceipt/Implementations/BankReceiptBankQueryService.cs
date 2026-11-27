using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.PaymentPool.Persistence.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptBankQueryService : AbstractBaseConnection, IBankReceiptBankQueryService
    {
        private const string SelectBanks = "USE [Atlas]; SELECT Id, BankName, [_3Char], Icon, CentralBankCode, Description FROM [PaymentPool].[Bank]";

        public BankReceiptBankQueryService(IConfiguration configuration) : base(configuration) { }

        public async Task<Bank> Get(short id)
        {
            using var connection = _sqlReportConnection;
            return await connection.QuerySingleOrDefaultAsync<Bank>(SelectBanks + " WHERE Id=@id", new { id })
                ?? throw new BaseException("بانک یافت نشد.");
        }

        public async Task<ICollection<Bank>> Get()
        {
            using var connection = _sqlReportConnection;
            return (await connection.QueryAsync<Bank>(SelectBanks)).AsList();
        }
    }
}
