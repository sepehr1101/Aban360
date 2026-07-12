using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.Dms;
using Dapper;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Dms.Commands
{
    public sealed class RequestDiscountCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public RequestDiscountCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(ClientDiscountInsertDto input)
        {
            string command = GetInsertCommand();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertClientDiscount);
            }
        }
        public async Task Update(ClientDiscountUpdateDto input)
        {
            string command = GetUpdateCommand();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateClientDiscount);
            }
        }
        public async Task UpdateByCodeMeli(ClientDiscountInsertDto input)
        {
            string command = GetUpdateByCodeMeliCommand();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateClientDiscount);
            }
        }
        
        public async Task Remove(ClientDiscountRemoveDto input)
        {
            string command = GetRemoveCommand();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveClientDiscount);
            }
        }

        private string GetInsertCommand()
        {
            return @"Insert AbAndFazelab.dbo.ClientDiscount(
                    	FileName,Name,Family,FatherName,CodeMeli,
                    	DarsadJanbazi,Radif,PreRadif,LifeType)
                    Values(
                        @FileName,@Name,@Family,@FatherName,@CodeMeli,
                    	@DarsadJanbazi,@Radif,@PreRadif,@LifeType)";
        }
        private string GetUpdateCommand()
        {
            return @"Update AbAndFazelab.dbo.ClientDiscount
                    Set
                    	Name=@Name ,Family=@Family ,FatherName=@FatherName ,CodeMeli=@CodeMeli ,
                    	DarsadJanbazi=@DarsadJanbazi ,Radif=@Radif ,PreRadif=@PreRadif ,LifeType=@LifeType
                    Where Id=@Id";
        }
        private string GetUpdateByCodeMeliCommand()
        {
            return @"UPDATE AbAndFazelab.dbo.ClientDiscount
             SET 
                 FileName = @FileName,
                 Name = @Name,
                 Family = @Family,
                 FatherName = @FatherName,
                 DarsadJanbazi = @DarsadJanbazi,
                 Radif = @Radif,
                 PreRadif = @PreRadif,
                 LifeType = @LifeType
             WHERE CodeMeli = @CodeMeli";
        }
        private string GetRemoveCommand()
        {
            return @"Update AbAndFazelab.dbo.ClientDiscount
                    Set
                    	RemovedBy=@RemovedBy , RemovedDateTime=@RemovedDateTime
                    Where Id=@Id";
        }
    }
}