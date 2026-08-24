using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class CustomerInfoDetailQueryService : AbstractBaseConnection, ICustomerInfoDetailQueryService
    {
        public CustomerInfoDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<CustomerInfoOutputDto> GetInfo(string billId, string readingDate="")
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await GetZoneIdCustomerNumber(billId);

            string dbName = GetDbName(zoneIdAndCustomerNumber.ZoneId);
            string query = GetCustomerInfoDataQuery(dbName);
            CustomerInfoOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerInfoOutputDto>(query, new { zoneId = zoneIdAndCustomerNumber.ZoneId, customerNumber = zoneIdAndCustomerNumber.CustomerNumber });
            try
            {
                if(string.IsNullOrWhiteSpace(readingDate))
                {
                    readingDate = DateTime.Now.ToShortPersianDateString();
                }
                result.HouseholdNumber= GetHouseholdUnit(result.HouseholdNumber, result.HouseholdDate, readingDate);
            }
            catch
            {
            }
            return result;
        }
        public async Task<ZoneIdAndCustomerNumberOutputDto> GetZoneIdCustomerNumber(string billId)
        {
            string query = GetZoneIdQuery();
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberOutputDto>(query, new { billId });
            if (zoneIdAndCustomerNumber == null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound + billId);
            }

            return zoneIdAndCustomerNumber;
        }

        private string GetCustomerInfoDataQuery(string dataBaseName)
        {
            return @$"Select
                        (TRIM(m.name) + ' ' + TRIM(m.family)) as FullName,
	                    t51.C2 as ZoneTitle,
                    	m.town as ZoneId,
                    	m.radif as Radif,
						Trim(m.bill_id) as BillId,
                    	m.noe_va as BranchType,
                    	m.cod_enshab as UsageId,
	                    t41.C1 as UsageTitle,
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
                    Left Join [Db70].dbo.T41 t41 
                    	ON m.cod_enshab	= t41.C0
                    Left Join [Db70].dbo.T51 t51
                    	ON m.town = t51.C0
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
        private int GetHouseholdUnit(int householdUnit, string? householdDate, string readingDateJalali)
        {
            if (householdUnit <= 0)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(householdDate))
            {
                return 0;
            }
            DateTime? expireHouseHoldGregorian = householdDate.ToGregorianDateTime();
            if (!expireHouseHoldGregorian.HasValue)
            {
                return 0;
            }
            DateTime? readingDateGregorian = readingDateJalali.ToGregorianDateTime();
            if (!readingDateGregorian.HasValue)
            {
                return 0;//throw new InvalidDateException(readingDateJalali);
            }
            if (readingDateGregorian.Value < expireHouseHoldGregorian.Value)//تاریخ قرائت قبل از تاریخ ثبت خانوار
            {
                return 0;
            }
            if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value)// تاریخ قرائت بعد از تاریخ انقضای خانوار
            {
                return 0;
            }
            return householdUnit;
        }
    }
}
