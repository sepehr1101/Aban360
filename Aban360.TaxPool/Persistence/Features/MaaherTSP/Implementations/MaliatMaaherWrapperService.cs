using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Implementations
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
        public async Task UpdateAmountAndCount(MaliatMaaherWrapperAmountAndCountUpdateDto input)
        {
            string command = GetUpdateAmountAndCountCommand();
            await _sqlConnection.ExecuteAsync(command, input);
        }
        public async Task UpdateConfirmed(MaliatMaaherWrapperConfirmedUpdateDto input)
        {
            string command = GetUpdateConfirmedDateCommand();
            await _sqlConnection.ExecuteAsync(command, input);
        }
        public async Task UpdateSend(MaliatMaaherWrapperSendUpdateDto input)
        {
            string command = GetUpdateSendCommand();
            await _sqlConnection.ExecuteAsync(command, input);
        }
        public async Task<MaliatMaaherWrapperGetDto> Get(int id)
        {
            string query = GetQuery();
            MaliatMaaherWrapperGetDto result = await _sqlConnection.QueryFirstOrDefaultAsync<MaliatMaaherWrapperGetDto>(query, new { id });
            if (result is null || result.Id <= 0)
            {
                throw new TaxMaaherException(ExceptionLiterals.InvalidId);
            }

            return result;
        }
        public async Task<IEnumerable<MaliatMaaherWrapperGetDto>> Get()
        {
            string query = GetQueryAll();
            IEnumerable<MaliatMaaherWrapperGetDto> result = await _sqlConnection.QueryAsync<MaliatMaaherWrapperGetDto>(query, null);
            return result;
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
        private string GetUpdateConfirmedDateCommand()
        {
            return @"Update Aban360.TaxPool.MaliatMaaherWrapper
                    Set ConfirmedDateTime=@ConfirmedDateTime
                    Where Id=@Id";
        }
        private string GetUpdateSendCommand()
        {
            return @"Update Aban360.TaxPool.MaliatMaaherWrapper
                    Set SendDateTime=@SendDateTime , SendByUserId=@SendByUserId
                    Where Id=@Id";
        }
        private string GetQuery()
        {
            return @"Select *
                    From Aban360.TaxPool.MaliatMaaherWrapper
                    Where Id=@Id";
        }
        private string GetQueryAll()
        {
            return @"Select *
                    From Aban360.TaxPool.MaliatMaaherWrapper";
        }
    }
}
