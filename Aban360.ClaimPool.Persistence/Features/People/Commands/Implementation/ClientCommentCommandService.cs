using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Implementation
{
    internal sealed class ClientCommentCommandService : AbstractBaseConnection, IClientCommentCommandService
    {
        public ClientCommentCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Insert(ClientCommentInsertDto inputDto)
        {
            string command = GetInsertCommand();
            int recordEffect = await _sqlReportConnection.ExecuteAsync(command, inputDto);
            if (recordEffect <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidInsertComment);
            }
        }
        private string GetInsertCommand()
        {
            return @"Insert [CustomerWarehouse].dbo.ClientComments(
                        BillId,Comment,
                        UserDisplayName,UserId,
                        InsertDateTime)
                    Values(     
                        @BillId,@Comment,
                        @UserDisplayName,@UserId,
                        @InsertDateTime)";
        }
    }
}
