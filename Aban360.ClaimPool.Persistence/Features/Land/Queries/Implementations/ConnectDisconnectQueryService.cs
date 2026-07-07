using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.DbSeeder.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
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
        public async Task<IEnumerable<ConnectDisconnectMainDataOutputDto>> Get(ConnectDisconnectMainInputDto inputDto)
        {
            string query = GetMainReportQuery(false);
            IEnumerable<ConnectDisconnectMainDataOutputDto> result = await _sqlReportConnection.QueryAsync<ConnectDisconnectMainDataOutputDto>(query, inputDto);
            return result;
        }
        public async Task<IEnumerable<ConnectDisconnectMainByCompanyDataOutputDto>> GetWithCompany(ConnectDisconnectMainInputDto inputDto)
        {
            string query = GetMainReportQuery(true);
            IEnumerable<ConnectDisconnectMainByCompanyDataOutputDto> result = await _sqlReportConnection.QueryAsync<ConnectDisconnectMainByCompanyDataOutputDto>(query, inputDto);
            return result;
        }
        public async Task<IEnumerable<ConnectDisconnectDetailDataOutputDto>> Get(ConnectDisconnectDetailInputDto inputDto)
        {
            string query = GetDetailReportQuery(false);
            IEnumerable<ConnectDisconnectDetailDataOutputDto> result = await _sqlReportConnection.QueryAsync<ConnectDisconnectDetailDataOutputDto>(query, inputDto);
            return result;
        }
        public async Task<IEnumerable<ConnectDisconnectDetailByCompanyDataOutputDto>> GetWithCompany(ConnectDisconnectDetailInputDto inputDto)
        {
            string query = GetDetailReportQuery(true);
            IEnumerable<ConnectDisconnectDetailByCompanyDataOutputDto> result = await _sqlReportConnection.QueryAsync<ConnectDisconnectDetailByCompanyDataOutputDto>(query, inputDto);
            return result;
        }
        public async Task<IEnumerable<ConnectDisconnectVeryDetailDataOutputDto>> Get(ConnectDisconnectVeryDetailInputDto input)
        {
            string query = GetByDateConditionQuery();
            IEnumerable<ConnectDisconnectVeryDetailDataOutputDto> result = await _sqlReportConnection.QueryAsync<ConnectDisconnectVeryDetailDataOutputDto>(query, input);
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
	                    CompanyId,
	                    PersonnelId,
	                    PersonnelName,
	                    JudicialNoticeId,
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
<<<<<<< HEAD
                        CompanyId,
                        PersonnelId,
                        PersonnelName,
=======
	                    CompanyId,
	                    PersonnelId,
	                    PersonnelName,
	                    JudicialNoticeId,
>>>>>>> hotfix
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
<<<<<<< HEAD
                        CompanyId,
                        PersonnelId,
                        PersonnelName,
=======
	                    CompanyId,
	                    PersonnelId,
	                    PersonnelName,
	                    JudicialNoticeId,
>>>>>>> hotfix
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
        private string GetMainReportQuery(bool hasCompany)
        {
            string companyCondition = hasCompany ? " CompanyTitle, " : string.Empty;
            return $@"Select 
                    	ZoneTitle,
                        {companyCondition}
                    	TypeTitle,
                    	COUNT(1) Count
                    From CustomerWarehouse.dbo.ConnectDisconnect
                    Where 
                    	ZoneId IN @ZoneIds AND
                    	FORMAT(CAST(CommandDateTime AS DATE),'yyyy/MM/dd','fa') BETWEEN @FromDateJalali AND @ToDateJalali
                    GROUP BY 
                    	ZoneTitle,
                        {companyCondition}
                    	TypeTitle
                    ORDER BY 
                        ZoneTitle,
                        {companyCondition}
                        TypeTitle";
        }
        private string GetDetailReportQuery(bool hasCompany)
        {
            string companyCondition = hasCompany ? " CompanyTitle, " : string.Empty;
            return $@"Select 
                    	ZoneTitle,
                        {companyCondition}
                    	TypeTitle,
	                    CommandCauseTitle,
	                    ResultTitle,
                    	COUNT(1) Count
                    From CustomerWarehouse.dbo.ConnectDisconnect
                    Where 
                    	ZoneId = @ZoneId AND
                    	FORMAT(CAST(CommandDateTime AS DATE),'yyyy/MM/dd','fa') BETWEEN @FromDateJalali AND @ToDateJalali
                    GROUP BY 
                    	ZoneTitle,
                        {companyCondition}
                    	TypeTitle,
	                    CommandCauseTitle,
	                    ResultTitle
                    ORDER BY 
                        ZoneTitle,
                        {companyCondition}
                        TypeTitle,
	                    CommandCauseTitle,
	                    ResultTitle";
        }
        private string GetByDateConditionQuery()
        {
            return $@"Select 
                    	Id,
						BillId,
                    	ZoneId,
                    	ZoneTitle,
                    	WaterDebt,
                    	CommandDateTime,
                    	CommandBy,
                    	CommandCauseId,
                    	CommandCauseTitle,
                    	ResultDateTime,
                    	ResultBy,
                    	ResultId,
                    	ResultTitle,
                    	CompanyId,
                    	CompanyTitle,
                    	PersonnelId,
                    	PersonnelName,
                    	TypeId,
                    	TypeTitle, 
                    	JudicialNoticeId,
                    	RemovedBy,
                    	RemovedDateTime
                    From CustomerWarehouse.dbo.ConnectDisconnect
                    Where 
                    	ZoneId=@ZoneId AND
                    	FORMAT(CAST(CommandDateTime AS DATE),'yyyy/MM/dd','fa') BETWEEN @FromDateJalali AND @ToDateJalali ";
        }
    }
}
