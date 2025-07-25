﻿using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class EmptyUnitQueryService : AbstractBaseConnection, IEmptyUnitQueryService
    {
        public EmptyUnitQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>> GetInfo(EmptyUnitInputDto input)
        {
            string emptyUnitQuery = GetEmptyUnitQuery();
            var @params = new
            {
                input.FromReadingNumber,
                input.ToReadingNumber,
                input.FromEmptyUnit,
                input.ToEmptyUnit,

                UsageIds = input.UsageSellIds,
                input.ZoneIds
            };

            IEnumerable<EmptyUnitDataOutputDto> emptyUnitData = await _sqlReportConnection.QueryAsync<EmptyUnitDataOutputDto>(emptyUnitQuery, @params);
            EmptyUnitHeaderOutputDto emptyUnitHeader = new EmptyUnitHeaderOutputDto()
            {
                FromEmptyUnit = input.FromEmptyUnit,
                ToEmptyUnit = input.ToEmptyUnit,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Count() : 0,
                SumDomesticCount = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.DomesticUnit) : 0,
                SumEmptyUnit = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.EmptyUnit) : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };


            var result = new ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>(ReportLiterals.EmptyUnit, emptyUnitHeader, emptyUnitData);

            return result;
        }

        private string GetEmptyUnitQuery()
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
                        c.DeletionStateId,
                        c.DeletionStateTitle AS UseStateTitle,
                        c.DomesticCount DomesticUnit,
            	        c.CommercialCount CommercialUnit,
            	        c.OtherCount OtherUnit,
            	        TRIM(c.BillId) BillId,
            			c.EmptyCount As EmptyUnit,
                        c.ZoneId,
						c.ZoneTitle,
                        0 AS RegionId,
                        '-' AS RegionTitle,
						c.NationalId AS NationalCode,
						c.PostalCode , 
						c.PhoneNo AS PhoneNumber,
						c.FatherName 
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
            			c.ToDayJalali IS NULL AND
            			c.UsageId in @UsageIds AND
                        c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber AND
                        c.ZoneId in @ZoneIds AND
            			c.EmptyCount BETWEEN @FromEmptyUnit AND @ToEmptyUnit";
        }
    }
}
