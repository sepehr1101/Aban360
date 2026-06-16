using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
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
        public async Task<IEnumerable<ConnectDisconnectDataOutputDto>> Get(int zoneId, bool isNoResult, bool isNoRemoved)
        {
            string noResultCondition = isNoResult ? "AND d.ResultId Is Null" : string.Empty;
            string removedCondition = isNoRemoved ? "AND d.RemovedDateTime Is Null" : string.Empty;
            string query = GetByConditionQuery(noResultCondition, removedCondition);
            IEnumerable<ConnectDisconnectDataOutputDto> result = await _sqlReportConnection.QueryAsync<ConnectDisconnectDataOutputDto>(query, new { zoneId });
            return result;
        }
        public async Task<ConnectDisconnectGetDto> Get(long id, int? typeId)
        {
            string typeCondition = typeId is null ? string.Empty : "AND TypeId=@typeId";
            string query = GetByIdQuery(typeCondition);
            ConnectDisconnectGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ConnectDisconnectGetDto>(query, new { id, typeId });
            if (result is null)
            {
                throw new InvalidDataException(ExceptionLiterals.InvalidConnectDisconnectId);
            }
            return result;
        }
        public async Task<ConnectDisconnectGetDto?> Get(ConnectDisconnectGetWithConditionDto inputDto)
        {
            string query = GetByConditionQuery(inputDto.IsRemoved, inputDto.HasResult);
            ConnectDisconnectGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ConnectDisconnectGetDto>(query, inputDto);
            return result;
        }
        private string GetQuery()
        {
            return @$"Select 
                        Id , 
                        ZoneId , 
                        ZoneTitle , 
                        BillId , 
                        WaterDebt , 
                        CommandDateTime , 
                        CommandBy , 
                        CommandCauseId , 
                        CommandCauseTitle , 
                        ResultDateTime ,
                        ResultBy ,
                        ResultId ,
                        ResultTitle ,
                        MeterDiameterId ,
                        MeterDiameterTitle , 
                        CompanyTitle ,
                        TypeId , 
                        TypeTitle ,
                        Description ,
                        RemovedDateTime ,
                        RemovedBy
                    From [CustomerWarehouse].dbo.connectdisconnect";
        }
        private string GetByConditionQuery(string noResultCondition, string removedCondition)
        {
            return @$"Select 
                        d.Id , 
                        d.ZoneId , 
                        d.ZoneTitle , 
                        d.BillId , 
                        d.WaterDebt , 
                        d.CommandDateTime , 
                        d.CommandBy , 
                        d.CommandCauseId , 
                        d.CommandCauseTitle , 
                        d.ResultDateTime ,
                        d.ResultBy ,
                        d.ResultId ,
                        d.ResultTitle ,
                        d.MeterDiameterId ,
                        d.MeterDiameterTitle , 
                        d.CompanyTitle ,
                        d.TypeId , 
                        d.TypeTitle ,
                        d.Description ,
                        d.RemovedDateTime ,
                        d.RemovedBy ,
						TRIM(c.FirstName) FirstName ,
						TRIM(c.SureName) SurName,
						TRIM(c.FirstName) + ' '+ TRIM(c.SureName) FullName,
						TRIM(c.MobileNo) MobileNumber
                    From [CustomerWarehouse].dbo.connectdisconnect d
					Join [CustomerWarehouse].dbo.Clients c
						ON d.BillId Collate Arabic_CI_AS=c.BillId
                    Where 
						c.ToDayJalali Is Null AND
						d.ZoneId=@zoneId
                        {noResultCondition}
                        {removedCondition}";
        }
        private string GetByIdQuery(string typeCondition)
        {
            return @$"Select 
                        Id , 
                        ZoneId , 
                        ZoneTitle , 
                        BillId , 
                        WaterDebt , 
                        CommandDateTime , 
                        CommandBy , 
                        CommandCauseId , 
                        CommandCauseTitle , 
                        ResultDateTime ,
                        ResultBy ,
                        ResultId ,
                        ResultTitle ,
                        MeterDiameterId ,
                        MeterDiameterTitle , 
                        CompanyTitle ,
                        TypeId , 
                        TypeTitle ,
                        Description ,
                        RemovedDateTime ,
                        RemovedBy
                    From [CustomerWarehouse].dbo.connectdisconnect
                    Where 
                        Id=@id 
                        {typeCondition}";
        }
        private string GetByConditionQuery(bool isRemoved, bool hasResult)
        {
            string removedCondition = isRemoved ? " AND RemovedDateTime IS NOT NUll " : " AND RemovedDateTime IS NUll ";
            string resultCondition = hasResult ? " AND ResultDateTime IS NOT NULL " : " AND ResultDateTime IS NULL ";
            return @$"Select
                        Id , 
                        ZoneId , 
                        ZoneTitle , 
                        BillId , 
                        WaterDebt , 
                        CommandDateTime , 
                        CommandBy , 
                        CommandCauseId , 
                        CommandCauseTitle , 
                        ResultDateTime ,
                        ResultBy ,
                        ResultId ,
                        ResultTitle ,
                        MeterDiameterId ,
                        MeterDiameterTitle , 
                        CompanyTitle ,
                        TypeId , 
                        TypeTitle ,
                        Description ,
                        RemovedDateTime ,
                        RemovedBy
                    From [CustomerWarehouse].dbo.connectdisconnect
                    Where 
                        BillId=@BillId AND
						TypeId=@TypeId 
						{resultCondition}
                        {removedCondition}";
        }
    }
}
