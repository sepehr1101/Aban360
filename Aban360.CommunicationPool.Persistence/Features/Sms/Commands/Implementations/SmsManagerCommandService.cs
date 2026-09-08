using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.CommunicationPool.Domain.Features.Sms.Commands;
using Dapper;
using System.Data;

namespace Aban360.CommunicationPool.Persistence.Features.Sms.Commands.Implementations
{
    public sealed class SmsManagerCommandService
    {
        private readonly IDbConnection _sqlRonnection;
        private readonly IDbTransaction _transaction;
        public SmsManagerCommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _sqlRonnection = sqlRonnection;
            _sqlRonnection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(SmsManagerInsertDto inputDto)
        {
            string command = GetInsertQuery();
            int recordCount = await _sqlRonnection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidSmsManagerInsert);
            }
        }
        public async Task Insert(IEnumerable<SmsManagerInsertDto> inputDtos)
        {
                DataTable table=GetInsertDataTable(inputDtos);
        }
        private DataTable GetInsertDataTable(IEnumerable<SmsManagerInsertDto> inputDtos)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("TrackNumber", typeof(int));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("ReferenceId", typeof(string));
            table.Columns.Add("TypeId", typeof(int));
            table.Columns.Add("TypeTitle", typeof(string));
            table.Columns.Add("InsertDateTime", typeof(DateTime));
            table.Columns.Add("FetchDateTime", typeof(DateTime));
            table.Columns.Add("SendDateTime", typeof(DateTime));

            foreach (var item in inputDtos)
            {
                var row = table.NewRow();

                row["Id"] = item.Id;
                row["TrackNumber"] = item.TrackNumber ?? (object)DBNull.Value;
                row["BillId"] = item.BillId ?? (object)DBNull.Value;
                row["ReferenceId"] = item.ReferenceId;
                row["TypeId"] = item.TypeId;
                row["TypeTitle"] = item.TypeTitle;
                row["InsertDateTime"] = item.InsertDateTime;
                row["FetchDateTime"] = item.FetchDateTime ?? (object)DBNull.Value;
                row["SendDateTime"] = item.SendDateTime ?? (object)DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }
        private string GetInsertQuery()
        {
            return @"Insert Into CustomerWarehouse.dbo.SmsManager
                    (
                    	Id,TrackNumber, BillId,
                    	ReferenceId, TypeId, TypeTitle,
                    	InsertDateTime, FetchDateTime, SendDateTime
                    )
                    Values
                    (
                    	@Id, @TrackNumber, @BillId,
                    	@ReferenceId, @TypeId, @TypeTitle,
                    	@InsertDateTime, @FetchDateTime, @SendDateTime
                    )";
        }
    }
}
