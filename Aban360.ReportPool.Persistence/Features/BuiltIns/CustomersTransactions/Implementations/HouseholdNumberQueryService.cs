﻿using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HouseholdNumberQueryService : AbstractBaseConnection, IHouseholdNumberQueryService
    {
        public HouseholdNumberQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>> GetInfo(HouseholdNumberInputDto input, string lastYearJalali)
        {
            string householdNumberQuery = GetHouseholdNumberQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromHouseholdDateJalali = input.FromHouseholdDateJalali,
                toHouseholdDateJalali = input.ToHouseholdDateJalali,

                lastYearDate = lastYearJalali,
                zoneIds = input.ZoneIds
            };

            IEnumerable<HouseholdNumberDataOutputDto> householdNumberData = await _sqlReportConnection.QueryAsync<HouseholdNumberDataOutputDto>(householdNumberQuery,@params);
            HouseholdNumberHeaderOutputDto householdNumberHeader = new HouseholdNumberHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromHouseholdDateJalali = input.FromHouseholdDateJalali,
                ToHouseholdDateJalali = input.ToHouseholdDateJalali,
                RecordCount = (householdNumberData is not null && householdNumberData.Any()) ? householdNumberData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>(ReportLiterals.HouseholdNumber, householdNumberHeader, householdNumberData);

            return result;
        }

        private string GetHouseholdNumberQuery()
        {
            return @"SELECT 
                        c.CustomerNumber,
                        c.ReadingNumber,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) As Surname,
                        c.UsageTitle,
                        c.WaterDiameterTitle MeterDiameterTitle,
                        c.RegisterDayJalali AS EventDateJalali,
                        TRIM(c.Address) AS Address,
                        c.ZoneTitle,
                        c.DeletionStateId,
                        c.DeletionStateTitle AS UseStateTitle,
                        c.DomesticCount DomesticUnit,
	                    c.CommercialCount CommercialUnit,
	                    c.OtherCount OtherUnit,
	                    TRIM(c.BillId) BillId,
	            		c.HouseholdDateJalali As HouseholdDateJalali,
	            		c.FamilyCount AS HouseholdCount,
                        IIF(c.HouseholdDateJalali >@lastYearDate , 1 , 0) AS IsValid
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
	            		c.ToDayJalali IS NULL AND
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                        c.ZoneId in @zoneIds AND
	            		c.HouseholdDateJalali BETWEEN @fromHouseholdDateJalali AND @toHouseholdDateJalali";
        }
    }
}
