using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
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
            string query = GetQuery();
            IEnumerable<CollectBillsDataDto> data = await _sqlReportConnection.QueryAsync<CollectBillsDataDto>(query, new { FromDateJalali = input.FromDateJalali, ToDateJalali = input.ToDateJalali });

            return data;
        }
		//todo: remove CustomerNumberCondition From Cte
        private string GetQuery()//todo: Add some Prop  :SewageDiameter
        {
            return @$";With Clients As
					(
						SELECT 
						    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
						    *
						From [CustomerWarehouse].dbo.Clients c
						Where c.CustomerNumber<>0 And CustomerNumber=11304328
					)
					Select 
						CONCAT(
							c.ZoneTitle, ';', --ZoneTitle,
							'', ';',--ZoneAddress,
							'122', ';',--EmergencyContactNumber,
							666 ,' ' Collate Latin1_General_BIN2 ,';',
							IIF(LEN(TRIM(c.ReadingNumber))>3,SUBSTRING(TRIM(c.ReadingNumber),1,2),'00'), ';',--MadoodeQeraat,
							REPLACE(SUBSTRING(B.RegisterDay,1,7),'/','-'), ';',--Cycle,
							IIF(5=5,N'خوداظهاری',N'قرائت دوره ای'), ';',
							b.RegisterDay, ';', --IssueDate,
							'', ';',--NextReadingDate,
							b.OldDbSerial, ';',--BillSerialNumber,
							c.ReadingNumber, ';',--SubscriptionNumber,
							c.CustomerNumber, ';',--FileNumber,
							Replace( TRIM(c.FirstName)+' '+trim(c.SureName),';',''), ';',--FullName,
							REPLACE(Trim(c.Address),';',''), ';',--Address,
							 c.PostalCode, ';', --ZipCode, ';',
							 c.MeterSerialBody, ';', --CounterSerialNumber, ';',
							 b.CounterStateTitle, ';', --CounterStatus, ';',
							 c.DomesticCount, ';', --ResidentialPurchased, ';',
							 (c.DomesticCount- c.EmptyCount), ';', --ResidentialOccupied, ';',
							 (c.CommercialCount + c.OtherCount), ';', --NonResidentialPurchased, ';',
							 c.FamilyCount, ';', --FamilyCount, ';',
							 c.UsageTitle, ';', --Tariff, ';',
							 c.WaterDiameterTitle	, ';', --WaterDiameter, ';',
							 ' ', ';', --SewageDiameter, ';',--todo: do or not?
							 c.ContractCapacity, ';', --Capacity, ';',
							 b.PreviousDay, ';', --PreviousReadingDate, ';',
							 b.NextDay, ';', --CurrentReadingDate, ';',
							 b.Duration, ';', --Days, ';',
							 b.PreviousNumber, ';', --PreviousCounterDigit, ';',
							 b.NextNumber, ';', --CurrentCounterDigit, ';', 
							 b.Consumption, ';', --Consumption, ';',
							 b.ConsumptionAverage, ';', --AverageConsumption, ';',
							 0, ';', --AllowedConsumption, ';', --todo? (b.Consumption- b.masjar)
							 0, ';', --ExtraConsumption, ';',  
							 b.PreDebt, ';', --PreviousDebt , ';',  
							 b.Item16, ';', --BudgetLawToll, ';',
							 0, ';', --WaterCostNote2, ';',  --todo? b.ab_20
							 0, ';', --WaterCostNote3, ';', --todo? b.TAB_ABN_A
							 b.Item3, ';', --WaterSubscription, ';',
							 b.Item1, ';', --WaterCost, ';',
							 b.Item11, ';', --WarmWaterCost, ';',
							 b.Item12, ';', --ExtraWaterCost, ';',
							 0, ';', --WaterSubscriptionNote3, ';',
							 0, ';', --WaterArticle7, ';',
							 b.Item2, ';', --SewageCost, ';',
							 b.Item4, ';', --SewageSubscription, ';',
							 0, ';', --SewageCostNote3, ';', --todo? b.TAB_ABN_F
							 0, ';', --SewageSubscriptionNote3, ';',
							 0, ';', --SewageArticle7, ';',
							 b.Item5, ';', --ValueAddedTax, ';',
							 0, ';', --WaterBranchInstallmentCost, ';', --todo? b.C200
							 0, ';', --SewageInstallmentCost, ';',
							 0, ';', --ServiceInstallmentCost, ';',
							 0, ';', --WaterInstallmentCost, ';',
							 0, ';', --OtherCostsDescription, ';',
							 0, ';', --OtherCostsAmount, ';',
							 b.Payable, ';', --InvoiceSum, ';', --todo b.pard?
							 b.SumItems, ';', --todo b.baha?
							 b.Payable, ';', --todo b.pard?
							 ' ', ';', --AmountString, ';',
							 b.Deadline, ';', --PaymentDate, ';',
							 ' ', ';', --BillMessage, ';',
							 trim(b.BillId), ';', --BillID, ';',
							 trim(b.PayId), ';', --PaymentID, ';',
							 (RIGHT('0000000000000'+ISNULL( trim(B.BillId),''),13) Collate Latin1_General_BIN2 + RIGHT('0000000000000'+ISNULL(trim(B.PayId),''),13)) Collate Latin1_General_BIN2 , ';', --Barcode,
							 Trim(c.MobileNo),';', --MobileNumber,
						     IIF(LEN(TRIM(c.NationalId)) in (10,11),TRIM(c.NationalId),''),';', --cod meli,
							 k.StringCode,';',--tarefe 4 char, karbari
							 IIF(z.StringCode is NULL, '00',SUBSTRING(z.StringCode,1,2)),';',--ostan provinceCode
							 IIF(z.StringCode is NULL, '00',SUBSTRING(z.StringCode,3,2)),';',-- shahrestan
							 IIF(z.StringCode is NULL, '00',SUBSTRING(z.StringCode,5,2)),';',-- baxsh
							 IIF(z.StringCode is NULL, '0000',SUBSTRING(z.StringCode,7,4)),';',-- shahr - dehestan
							 IIF(v.StringCode is NULL, '0000000',v.StringCode),';',-- abadi roosta
							 k.StringCode,';',                                   -- new: 4 char tarefe
							 IIF(tg.StringCode is null,'000',tg.StringCode),';', -- new: 3 char coding dastgah ejraii parent
							 IIF(t.StringCode is null,'0000',t.StringCode),';',	 -- new: 4 char coding dastah ejraii child
							 IIF(c.ZoneId>140000,1,2),';',-- shahr ya roosta
							 2,';',--movaqat daem 75 BranchTypeCode
							 IIF(c.UsageId in (1,3), c.UsageId ,2),';', --CalculationTypeCode
							 IIF(c.SewageRegisterDateJalali>'1330/01/01',3,1),';',--ServiceTypeCode --todo m.G_inst_fas?
							 1,';',--BranchStatusCode 1:daier
							 IIF(b.CounterStateCode in (4,7,8),2,1),';', --ReadingStatusCode
							 1,';', --BillStatusCode
							 1,';',--ReadingTypeCode   
							 IIF(b.CounterStateCode in (1),2,1),';',--CounterStatusCode
							 IIF(b.CounterStateCode in (8),2,1),';',--BillKindCode
							 1,';',-- CityCoefficient
							 b.DomesticCount+b.CommercialCount+b.OtherCount,';',--CalcUnits
							 b.ContractCapacity,';', --SewageCapacity
							 b.Item10,';',--javani YouthPopulation --todo? zarib_d
							 '',';',--Reserve1
							 '',';',--Reserve2
							 '',';',--Reserve3
							 0,';',--Reserve4
							 0,';',--Reserve5
							 0--Reserve6    
						 )AS 'Row'
					From Clients c
					Join CustomerWarehouse.dbo.Bills b
						On c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
					Join CounterReadingTest01.dbo.CounterState cs--?
						On b.CounterStateCode=cs.MoshtarakinId and b.ZoneId=cs.ZoneId
					Join [Db70].dbo.T41 k
						On c.UsageId=k.C0
					Join Db70.dbo.T51 z
						On c.ZoneId=z.C0
					LEFT JOIN Db70.dbo.Village v
						On c.ZoneId=v.ZoneId and c.VillageId=v.VillageId
					LEFT JOIN CustomerWarehouse.dbo.BillIdTags bt
						On TRIM(b.BillId) COLLATE SQL_Latin1_General_CP1_CI_AS =TRIM(bt.BillId)
					LEFT JOIN  CustomerWarehouse.dbo.Tags t
						On bt.Id=bt.TagId
					LEFT JOIN CustomerWarehouse.dbo.TagGroups tg
						On t.TagGroupId=tg.Id and tg.MainTagGroupId=11
					Where 
						c.RN=1 AND
						b.CounterStateCode NOT IN (4,7,8) AND
						b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
						cs.IsActive=1 AND
						TRIM(b.BillId)<>'' AND
						TRIM(b.PayId)<>'' AND 
						b.Payable>10000 AND 
						LEN(TRIM(c.MobileNo))=11 
						AND b.Deadline>'1400/01/01'";
        }
    }
}
