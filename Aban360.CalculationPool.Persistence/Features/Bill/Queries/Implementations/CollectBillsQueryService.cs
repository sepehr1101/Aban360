using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public sealed class CollectBillsQueryService : AbstractBaseConnection, ICollectBillsQueryService
    {
        public CollectBillsQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<CollectBillsDataDto>> Get(CollectBillsGetDataToSendInputDto input)
        {
            List<CollectBillsDataDto> allData = new List<CollectBillsDataDto>();
            foreach (var singleZone in input.ZoneInfo)
            {
                string query = GetQuery(singleZone.DbName);
                IEnumerable<CollectBillsDataDto> data = await _sqlReportConnection.QueryAsync<CollectBillsDataDto>(query, new { FromDateJalali = input.FromDateJalali, ToDateJalali = input.ToDateJalali });
                allData.AddRange(data);
            }

            return allData;
        }
        private string GetQuery(string dbName)
        {
            return @$"use [{dbName}]
						select top 10
						   CONCAT(z.C2, ';', --ZoneTitle,
							'', ';',--ZoneAddress,
							'122', ';',--EmergencyContactNumber,
							b.mamor ,' ' Collate Latin1_General_BIN2 ,';',
							IIF(LEN(TRIM(m.eshtrak))>3,SUBSTRING(TRIM(m.eshtrak),1,2),'00'), ';',--MadoodeQeraat,
							REPLACE(SUBSTRING(B.date_bed,1,7),'/','-'), ';',--Cycle,
							IIF(b.operator=5,N'خوداظهاری',N'قرائت دوره ای'), ';',--BillKind,
							b.date_bed, ';', --IssueDate,
							'', ';',--NextReadingDate,
							b.serial, ';',--BillSerialNumber,
							m.eshtrak, ';',--SubscriptionNumber,
							m.radif, ';',--FileNumber,
							Replace( TRIM(m.name)+' '+trim(m.family),';',''), ';',--FullName,
							REPLACE(Trim(m.address),';',''), ';',--Address,
							 m.POST_COD, ';', --ZipCode, ';',
							 m.serial_co, ';', --CounterSerialNumber, ';',
							 c.Title, ';', --CounterStatus, ';',
							 m.tedad_mas, ';', --ResidentialPurchased, ';',
							 (m.tedad_mas - m.Khali_s), ';', --ResidentialOccupied, ';',
							 (m.tedad_tej+m.tedad_vahd), ';', --NonResidentialPurchased, ';',
							 m.ted_khane, ';', --FamilyCount, ';',
							 k.c1, ';', --Tariff, ';',
							 q.C2, ';', --WaterDiameter, ';',
							 ' ', ';', --SewageDiameter, ';',
							 m.fix_mas, ';', --Capacity, ';',
							 b.pri_date, ';', --PreviousReadingDate, ';',
							 b.today_date, ';', --CurrentReadingDate, ';',
							 b.modat, ';', --Days, ';',
							 b.pri_no, ';', --PreviousCounterDigit, ';',
							 b.today_no, ';', --CurrentCounterDigit, ';',
							 b.masraf, ';', --Consumption, ';',
							 b.rate, ';', --AverageConsumption, ';',
							 (b.masraf- b.masjar), ';', --AllowedConsumption, ';',
							 b.masjar, ';', --ExtraConsumption, ';',
							 (b.jam -b.baha), ';', --PreviousDebt , ';',
							 b.bodjeh, ';', --BudgetLawToll, ';',
							 b.ab_20, ';', --WaterCostNote2, ';',
							 b.TAB_ABN_A, ';', --WaterCostNote3, ';',
							 b.abon_ab, ';', --WaterSubscription, ';',
							 b.ab_baha, ';', --WaterCost, ';',
							 b.zaribfasl, ';', --WarmWaterCost, ';',
							 b.ztadil, ';', --ExtraWaterCost, ';',
							 0, ';', --WaterSubscriptionNote3, ';',
							 0, ';', --WaterArticle7, ';',
							 b.fas_baha, ';', --SewageCost, ';',
							 b.abon_fas, ';', --SewageSubscription, ';',
							 b.TAB_ABN_F, ';', --SewageCostNote3, ';',
							 0, ';', --SewageSubscriptionNote3, ';',
							 0, ';', --SewageArticle7, ';',
							 b.shahrdari, ';', --ValueAddedTax, ';',
							 b.C200, ';', --WaterBranchInstallmentCost, ';',
							 0, ';', --SewageInstallmentCost, ';',
							 0, ';', --ServiceInstallmentCost, ';',
							 0, ';', --WaterInstallmentCost, ';',
							 0, ';', --OtherCostsDescription, ';',
							 0, ';', --OtherCostsAmount, ';',
							 b.pard, ';', --InvoiceSum, ';',
							 b.baha, ';',
							 b.pard, ';',
							 ' ', ';', --AmountString, ';',
							 b.mohlat, ';', --PaymentDate, ';',
							 ' ', ';', --BillMessage, ';',
							 trim(b.sh_ghabs1), ';', --BillID, ';',
							 trim(b.sh_pard1), ';', --PaymentID, ';',
							 (RIGHT('0000000000000'+ISNULL( trim(B.sh_ghabs1),''),13) + RIGHT('0000000000000'+ISNULL(trim(B.sh_pard1),''),13)), ';', --Barcode,
							 Trim(m.MOBILE),';', --MobileNumber,
						     IIF(LEN(TRIM(m.meli_cod)) in (10,11),TRIM(m.meli_cod),''),';', --cod meli,
							 k.StringCode,';',--tarefe 4 char, karbari
							 IIF(z.StringCode is NULL, '00',SUBSTRING(z.StringCode,1,2)),';',--ostan provinceCode
							 IIF(z.StringCode is NULL, '00',SUBSTRING(z.StringCode,3,2)),';',-- shahrestan
							 IIF(z.StringCode is NULL, '00',SUBSTRING(z.StringCode,5,2)),';',-- baxsh
							 IIF(z.StringCode is NULL, '0000',SUBSTRING(z.StringCode,7,4)),';',-- shahr - dehestan
							 IIF(v.StringCode is NULL, '0000000',v.StringCode),';',-- abadi roosta
							 k.StringCode,';',                                   -- new: 4 char tarefe
							 IIF(tg.StringCode is null,'000',tg.StringCode),';', -- new: 3 char coding dastgah ejraii parent
							 IIF(t.StringCode is null,'0000',t.StringCode),';',	 -- new: 4 char coding dastah ejraii child
							 IIF(m.town>140000,1,2),';',-- shahr ya roosta
							 2,';',--movaqat daem 75 BranchTypeCode
							 IIF(m.cod_enshab in (1,3), m.cod_enshab ,2),';', --CalculationTypeCode
							 IIF(m.G_inst_fas>'1330/01/01',3,1),';',--ServiceTypeCode
							 1,';',--BranchStatusCode 1:daier
							 IIF(b.cod_vas in (4,7,8),2,1),';', --ReadingStatusCode
							 1,';', --BillStatusCode
							 IIF(b.operator=5,2,1),';',--ReadingTypeCode
							 IIF(b.cod_vas in (1),2,1),';',--CounterStatusCode
							 IIF(b.cod_vas in (8),2,1),';',--BillKindCode
							 1,';',-- CityCoefficient
							 b.tedad_mas+b.tedad_tej+b.tedad_vahd,';',--CalcUnits
							 b.fix_mas,';', --SewageCapacity
							 b.zarib_d,';',--javani YouthPopulation
							 '',';',--Reserve1
							 '',';',--Reserve2
							 '',';',--Reserve3
							 0,';',--Reserve4
							 0,';',--Reserve5
							 0--Reserve6    
							 )AS 'Row'
						from members m
						join bed_bes b
						on m.radif=b.radif and m.town=b.town
						join [Db70].dbo.T41 k
						on m.cod_enshab=k.C0
						join Db70.dbo.T51 z
						on m.town=z.C0
						join Db70.dbo.T46 x
						on z.C1=x.C0
						join CounterReadingTest01.dbo.CounterState c
						on b.cod_vas=c.MoshtarakinId and b.town=c.ZoneId
						JOIN Db70.dbo.T5 q
						on m.enshab=q.c1
						LEFT JOIN Db70.dbo.Village v
						on m.town=v.ZoneId and m.VillageId=v.VillageId
						LEFT JOIN CustomerWarehouse.dbo.BillIdTags bt
						on TRIM(b.sh_ghabs1) COLLATE SQL_Latin1_General_CP1_CI_AS =TRIM(bt.BillId)
						LEFT JOIN  CustomerWarehouse.dbo.Tags t
						on bt.Id=bt.TagId
						LEFT JOIN CustomerWarehouse.dbo.TagGroups tg
						on t.TagGroupId=tg.Id and tg.MainTagGroupId=11
						where b.cod_vas not in(4,7,8) and b.date_bed between @FromDateJalali and @ToDateJalali
						and c.IsActive=1 and TRIM(b.sh_ghabs1)<>'' and TRIM(b.sh_pard1)<>'' and pard>10000 and LEN(TRIM(m.MOBILE))=11 and b.mohlat>'1400/01/01'";
        }
    }
}
