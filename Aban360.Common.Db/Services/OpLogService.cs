using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace Aban360.Common.Db.Services
{
    public sealed class OpLogCommandService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public OpLogCommandService(
            IHttpContextAccessor contextAccessor,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(string text, IAppUser appUser)
        {
            LogInfo logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
            OpLogInsertDto insertDto = new()
            {
                UserId = appUser.UserId,
                DisplayName = appUser.FullName,
                UserName = appUser.Username,
                Ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                SystemInfo = JsonSerializer.Serialize(logInfo),
                Description = text,
            };

            string command = GetInsertCommand();
            int recordEffected = await _connection.ExecuteAsync(command, insertDto, _transaction);
            if (recordEffected <= 0)
            {
                throw new InvalidDateException(ExceptionLiterals.InvalidInsertOpLog);
            }
        }
        private string GetInsertCommand()
        {
            return $@"Insert CustomerWarehouse.dbo.OpLog(
                        UserId,DisplayName,UserName,
                        Ip,SystemInfo,RegisterDateTime,Description)
                    Values(
                        @UserId ,@DisplayName ,@UserName ,
                        @Ip ,@SystemInfo ,@RegisterDateTime ,@Description)";
        }

    }
    public interface IOpLogQueryService
    {
        Task<IEnumerable<OpLogDataDto>> Get(OpLogGetDto input);
    }
    public sealed class OpLogQueryService : AbstractBaseConnection, IOpLogQueryService
    {
        public OpLogQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<OpLogDataDto>> Get(OpLogGetDto input)
        {
            string query = GetFromToDateQuery();
            DateTime fromDate = ConvertDate.JalaliToDateTime(input.FromDateJalali);
            DateTime toDate = ConvertDate.JalaliToDateTime(input.ToDateJalali);
            IEnumerable<OpLogDataDto> datas = await _sqlReportConnection.QueryAsync<OpLogDataDto>(query, new { fromDate, toDate });
            datas.ForEach(d => d.RegisterDateJalali = d.RegisterDateTime.ToShortPersianDateString());

            return datas;
        }
        private string GetFromToDateQuery()
        {
            return @"Select Top 10000
                    	Id,
                    	UserId,
                    	DisplayName,
                    	UserName,
                    	Ip,
                    	SystemInfo,
                    	RegisterDateTime,
                    	Description
                    From CustomerWarehouse.dbo.OpLog
                    Where 
                    CAST( DATEFROMPARTS( YEAR(RegisterDateTime), MONTH(RegisterDateTime), DAY(RegisterDateTime )) AS datetime)  BETWEEN @fromDate and @toDate ";
        }
    }
    public record OpLogInsertDto
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Ip { get; set; }
        public string SystemInfo { get; set; }
        public DateTime RegisterDateTime { get; set; } = DateTime.Now;
        public string Description { get; set; }
    }
    public record OpLogDataDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Ip { get; set; }
        public string SystemInfo { get; set; }
        public DateTime RegisterDateTime { get; set; }
        public string RegisterDateJalali { get; set; }
        public string Description { get; set; }
    }
    public record OpLogGetDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
