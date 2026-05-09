using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.AspNetCore.Http;
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
}
