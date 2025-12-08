using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts
{
    internal sealed class MaliatMaaherWrapperService : AbstractBaseConnection, IMaliatMaaherWrapperService
    {
        public MaliatMaaherWrapperService(IConfiguration configuraion)
            : base(configuraion)
        {
        }

        public async Task<int> Insert(MaliatMaaherWrapperInsertDto input)
        {
            string command = GetInsertCommand();
            int newWrapperId = await _sqlConnection.ExecuteScalarAsync<int>(command, input);
            if (newWrapperId <= 0)
            {
                throw new TaxMaaherException(ExceptionLiterals.InvalidMaaherWrapperInsert);
            }

            return newWrapperId;
        }
        public async Task UpdateAmountAndCount(UpdateMaliatMaaherWrapperAmountAndCountDto input)
        {
            string command = GetUpdateAmountAndCountCommand();
            await _sqlConnection.ExecuteAsync(command, input);
        }

        private string GetInsertCommand()
        {
            return @"INSERT INTO Aban360.TaxPool.MaliatMaaherWrapper(InsertDateTime,InsertByUserId,InvoiceCount,SumAmount)
                    VALUES(@InsertDateTime, @InsertByUserId, @InvoiceCount, @SumAmount);
    
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }
        private string GetUpdateAmountAndCountCommand()
        {
            return @"Update Aban360.TaxPool.MaliatMaaherWrapper
                    Set SumAmount=@SumAmount , InvoiceCount=@InvoiceCount
                    Where Id=@Id";
        }
    }
}
