using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class ConnectDisconnectQueryService : AbstractBaseConnection, IConnectDisconnectQueryService
    {
        public ConnectDisconnectQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<ConnectDisconnectGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<ConnectDisconnectGetDto> result = await _sqlReportConnection.QueryAsync<ConnectDisconnectGetDto>(query);
            return result;
        }

        public async Task<ConnectDisconnectGetDto> Get(long id)
        {
            string query = GetByIdQuery();
            ConnectDisconnectGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ConnectDisconnectGetDto>(query, new { id });
            if (result is null)
            {
                throw new InvalidDataException(ExceptionLiterals.InvalidConnectDisconnectId);
            }
            return result;
        }

        private string GetQuery()
        {
            return @"Select 
                        Id , 
                        ZoneId , 
                        ZoneTitle , 
                        BillId , 
                        WaterDebt , 
                        CommandDateTime , 
                        CommandBy , 
                        CommandCauseId , 
                        CommandCauseTitle , 
                        ResultDateTime 
                        ResultBy 
                        ResultId 
                        ResultTitle 
                        MeterDiameterId ,
                        MeterDiameterTitle , 
                        CompanyTitle 
                        TypeId , 
                        TypeTitle ,
                        Description 
                    From [CustomerWarehouse].dbo.connectdisconnect";
        }
        private string GetByIdQuery()
        {
            return @"Select 
                        Id , 
                        ZoneId , 
                        ZoneTitle , 
                        BillId , 
                        WaterDebt , 
                        CommandDateTime , 
                        CommandBy , 
                        CommandCauseId , 
                        CommandCauseTitle , 
                        ResultDateTime 
                        ResultBy 
                        ResultId 
                        ResultTitle 
                        MeterDiameterId ,
                        MeterDiameterTitle , 
                        CompanyTitle 
                        TypeId , 
                        TypeTitle ,
                        Description 
                    From [CustomerWarehouse].dbo.connectdisconnect
                    Where Id=@id";
        }
    }
}
