using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class CustomerInfoDetailQueryService : AbstractBaseConnection, ICustomerInfoDetailQueryService
    {
        public CustomerInfoDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<CustomerInfoOutputDto> GetInfo(string billId)
        {
            string zoneIdQueryString = GetZoneIdQuery();
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberOutputDto>(zoneIdQueryString, new { billId });
            if (zoneIdAndCustomerNumber == null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound+billId);
            }
            string DataBaseName = GetDbName(zoneIdAndCustomerNumber.ZoneId);
            string customerInfoQueryString = GetCustomerInfoDataQuery(DataBaseName);
            CustomerInfoOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerInfoOutputDto>(customerInfoQueryString, new { zoneId= zoneIdAndCustomerNumber.ZoneId, customerNumber= zoneIdAndCustomerNumber.CustomerNumber });

            return result;
        }


        private string GetCustomerInfoDataQuery(string dataBaseName)
        {
            return @$"Select
                    	m.town as ZoneId,
                    	m.radif as Radif,
						Trim(m.bill_id) as BillId,
                    	m.noe_va as BranchType,
                    	m.cod_enshab as UsageId,
                    	m.tedad_mas as DomesticUnit,
                    	m.tedad_tej as CommertialUnit,
                    	m.tedad_vahd as OtherUnit,
                    	m.inst_ab as WaterInstallationDateJalali,
                    	m.inst_fas as SewageInstallationDateJalali,
                        m.g_inst_ab WaterRegisterDate,
                        m.g_inst_fas SewageRegisterDate,
                    	m.n_ab as WaterCount,
                    	m.n_faz as SewageCalcState,
						m.fix_mas as ContractualCapacity,
                        m.ted_khane as HouseholdNumber,
                        m.date_KHANE as HouseholdDate,
						m.eshtrak as ReadingNumber,
                        m.VillageId as VillageId,
						m.edareh_k as IsSpecial,
						m.enshab as MeterDiameterId,
						m.Khali_s as EmptyUnit,
                        m.EJUCA as VirtualCategoryId
                    From [{dataBaseName}].dbo.members m
                    Where
                    	m.town=@zoneId AND 
						m.radif=@customerNumber";
        }
        private string GetZoneIdQuery()
        {
            return @"Select c.ZoneId,c.CustomerNumber
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.BillId=@billId AND
                    	c.ToDayJalali IS NULL";
        }
    }
}
