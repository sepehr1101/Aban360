using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class SubscriptionQueryService : AbstractBaseConnection, ISubscriptionQueryService
    {
        public SubscriptionQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<SubscriptionAssignmentGetDto> Get(string input)
        {
            var (@params,dbName)= await GetParamsAndDbName(input);

            string query = GetSubscriptionAssignmentQuery(dbName);
            SubscriptionAssignmentGetDto subscriptionAssignmentGetDto = await _sqlReportConnection.QueryFirstAsync<SubscriptionAssignmentGetDto>(query, @params);

            return subscriptionAssignmentGetDto;
        }
        public async Task<SubscriptionGetDto> GetInfo(string input)
        {
            var (@params, dbName) = await GetParamsAndDbName(input);

            string query = GetSubscriptionQuery(dbName);
            SubscriptionGetDto subscriptionAssignmentGetDto = await _sqlReportConnection.QueryFirstOrDefaultAsync<SubscriptionGetDto>(query,@params);

            return subscriptionAssignmentGetDto;
        }
        private async Task<(object,string)> GetParamsAndDbName(string input)
        {
            string zoneIdQuery = GetZoneIdQuery();
            ZoneIdCustomerNumber data = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdCustomerNumber>(zoneIdQuery, new { billId = input });
            if (data == null)
            {
                throw new BaseException(ExceptionLiterals.BillIdNotFound);
            }
            var @params = new { customerNumber = data.CustomerNumber, zoneId = data.ZoneId };
            string dbName = GetDbName(data.ZoneId);
            //string dbName = "Atlas";

            return (@params,dbName);
        }

        private string GetZoneIdQuery()
        {
            return @"Select 
                        c.ZoneId,
                        c.CustomerNumber
                     From  [CustomerWarehouse].dbo.Clients c
                     Where 
						c.BillId=@billId AND
						c.ToDayJalali IS NULL";
        }
        private string GetSubscriptionAssignmentQuery(string zoneId)
        {
            return @$"Select
                        m.Id,
                        m.X,
                    	m.Y,
						TRIM(m.bill_id) BillId,
                    	TRIM(m.name) AS FirstName,
                    	TRIM(m.family) AS SurName,
                    	TRIM(m.address) AS Address,
                    	TRIM(m.eshtrak) AS ReadingNumber,
						TRIM(m.POST_COD) AS PostalCode
                    From [{zoneId}].dbo.members m
                    Where
                        m.radif=@customerNumber AND
                        m.town=@zoneId";
        }
        private string GetSubscriptionQuery(string dbName)
        {
            return $@"Select
                        m.Id,
                        m.town ZoneId,
                        m.radif CustomerNumber,
                    	TRIM(m.bill_id) BillId,
                        m.X,
                    	m.Y,
                    	TRIM(m.name) FirstName,
                    	TRIM(m.family) SurName,
                    	TRIM(m.address) Address,
                    	TRIM(m.eshtrak) ReadingNumber,
                    	TRIM(m.POST_COD) PostalCode,
                    	TRIM(m.pelak) Plaque,
                    	TRIM(m.MELI_COD) NationalCode,
                    	TRIM(m.PHONE_NO) PhoneNumber,
                    	TRIM(m.MOBILE) MobileNumber,
                    	TRIM(m.father_nam) FatherName,
                    	m.noe_va BranchTypeId,
                    	m.enshab UsageSellId,
                    	m.group1 UsageConsumptionId,
                    	m.Khali_s EmptyUnit,
                    	m.tedad_tej CommertialUnit,
                    	m.tedad_mas DomesticUnit,
                    	m.tedad_vahd OtherUnit,
                    	m.ted_khane HouseholdNumber,
                    	TRIM(m.date_KHANE) HouseholdDateJalali,
                    	m.enshab MeterDiamterId,
                    	m.edareh_k IsSpecial,
                    	m.fix_mas ContractualCapacity,
                    	m.arse Premises,
                    	m.aian ImprovementOverall,
                    	m.aian_mas ImprovementDomestic,
                    	m.aian_tej ImprovementCommertial,
                    	TRIM(m.inst_ab) WaterInstallationDateJalali,
                    	TRIM(m.ask_ab) WaterRequestDateJalali ,
                    	TRIM(m.inst_fas) SewageInstallationDateJalali,
                    	TRIM(m.ask_fas) SewageRequestDateJalali ,
                    	m.sif_1 Siphon100,
                        m.sif_2 Siphon125,
                        m.sif_3 Siphon150,
                        m.sif_4 Siphon200,
                        m.sif_5 Siphon5,
                        m.sif_6 Siphon6,
                        m.sif_7 Siphon7,
                        m.sif_8 Siphon8,
                        m.master_sif MainSiphon,
                    	m.operator Operator
                    From [{dbName}].dbo.members m
                    Where
                        m.radif=@customerNumber AND
                        m.town=@zoneId";
        }
    }
}
