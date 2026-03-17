using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class BillCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public BillCommandService(
                IDbConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task InsertByBedBesId(ZoneIdAndCustomerNumber inputDto, int bedBesId, string dbName)
        {
            string command = GetInsertByBedBesCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, new { inputDto.CustomerNumber, inputDto.ZoneId, bedBesId }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidBillInsert);
            }
        }
        public async Task InsertReturnByRepair(int repairId, string dbName)
        {
            string command = GetInsertReturnByRepairCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, new { id = repairId }, _transaction);
            if (recordCount <= 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidSaveReturn);
            }
        }
        public async Task Delete(RemoveBillDto input)
        {
            string command = GetDeleteCommand();
            int recordCount = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidRemoveBill);
            }
        }

        private string GetInsertByBedBesCommand(string dbName)//todo:check
        {
            return $@"insert into [CustomerWarehouse].dbo.Bills 
                    select
                         b.town ZoneId,
                         z.C2 ZoneTitle,
                         b.radif CustomerNumber,
                         b.sh_ghabs1 BillId,
                         b.eshtrak ReadingNumber,
                         b.pri_no PreviousNumber,
                         b.today_no NextNumber,
                         b.pri_date PreviousDay,
                         b.today_date NextDay,
                         b.date_bed RegisterDay,
                         AbAndFazelab.dbo.PersianToMiladi(b.date_bed) RegisterDayGregorian,
                         IIF(c.Title IS NULL, '', c.Title) CounterStateTitle,
                         b.cod_enshab  AS  [UsageId], 
                         b.group1 [UsageId2], 
                         k1.C1  AS  [UsageTitle], 
                         k2.C1  AS  [UsageTitle2], 
                         va.C1  AS  [BranchType], 
                         b.enshab  AS  [WaterDiameterId],
                         q.C2  AS  [WaterDiameterTitle],
                         IIF(m.sif_1 IS NOT NULL,m.sif_1,0) AS  [Siphon100], 
                         IIF(m.sif_2 IS NOT NULL,m.sif_2,0), 
                         IIF(m.sif_3 IS NOT NULL,m.sif_3,0) AS  [Siphon150], 
                         IIF(m.sif_4 IS NOT NULL,m.sif_4,0), 
                         IIF(m.sif_5 IS NOT NULL,m.sif_5,0) AS  [Siphon5],   
                         IIF(m.sif_6 IS NOT NULL,m.sif_6,0),   
                         IIF(m.sif_7 IS NOT NULL,m.sif_7,0) AS  [Siphon7],   
                         IIF(m.sif_8 IS NOT NULL,m.sif_8,0),   
                         b.fix_mas ContractCapacity,
                         b.tedad_mas DomesticCount,
                         b.tedad_tej CommercialCount,
                         b.tedad_vahd OtherCount,
                         b.Khali_s EmptyCount,
                         b.masraf Consumption,
                         b.modat Duration,
                         b.rate ConsumptionAverage,
                         b.mohlat Deadline,
                         b.jam -b.baha PreDebt,
                         b.ab_baha Item1,
                         b.fas_baha Item2, 
                         b.abon_ab Item3, 
                         b.abon_fas Item4,
                         b.shahrdari Item5,
                         b.ab_10 Item6 ,
                         b.ab_20 Item7, 
                         b.jarime Item8, 
                         b.zabresani Item9,
                         b.zarib_d Item10,
                         b.zaribfasl Item11, 
                         b.ztadil Item12,
                         b.TAB_ABN_A Item13, 
                         b.TAB_ABN_F Item14,
                         b.TABS_FA Item15,
                         b.bodjeh Item16,
                         b.C200 Item17,
                         b.Avarez Item18,
                         b.baha SumItems,
                         b.pard Payable,
                         N'قبض' TypeId,
                         IIF(k.ab_baha IS NULL,0, k.ab_baha) ItemOff1,
                         IIF(k.fas_baha IS NULL,0, k.fas_baha), 
                         IIF(k.abon_ab IS NULL,0, k.abon_ab ) ItemOff3, 
                         IIF(k.abon_fas IS NULL,0, k.abon_fas),
                         IIF(k.shahrdari IS NULL,0, k.shahrdari) ItemOff5,
                         IIF(k.ab_10 IS NULL,0, k.ab_10 ) ,
                         0 ItemOff7, 
                         0, 
                         0 ItemOff9,
                         0,
                         IIF(k.ZARIBFASL IS NULL,0, k.ZARIBFASL ) ItemOff11, 
                         0,
                         IIF(k.TAB_ABN_A IS NULL,0, k.TAB_ABN_A) ItemOff13, 
                         IIF(k.TAB_ABN_F IS NULL,0, k.TAB_ABN_F),
                         0 ItemOff15,
                         ISNULL(k.bodjeh,0),
                         0 ItemOff17,
                         0 ItemOff18,
                         IIF(ABS(b.kasr_ha-b.baha)<1000 AND b.rate<5 AND b.rate>0,1,0) IsFree,
                         IIF(m.VillageId IS NOT NULL,m.VillageId,'' ) VillageId,
                         IIF(m.VillageName IS NOT NULL,m.VillageName,'') VillageName,
                         '' ZoneId2, 
                         IIF(b.mamor=888,N'خوداظهاری غیرحضوری',IIF(b.mamor=999,N'خوداظهاری حضوری',IIF(b.mamor=0,N'بدون کد مامور',N'دارای کد مامور'))) ReadingStateTitle,
                         b.sh_pard1 PayId,
                         b.cod_vas CounterStateCode,
                         1 TypeCode,
                         N'قبض' TypeTitle,
                         NULL ReturnCauseId,
                         NULL,
                         b.noe_va,
                         IIF(b.ghabs='2' AND b.cod_vas NOT IN(4,7,8),1,0) IsSettlement
                    FROM [{dbName}].dbo.bed_bes b
                    JOIN Db70.dbo.T51 z
                    	ON b.town=z.C0
                    LEFT OUTER JOIN [{dbName}].dbo.kasr_ha k
                    	ON b.radif=k.radif AND b.town=k.TOWN AND b.date_bed=k.date_bed AND b.barge=k.barge and k.del=0
                    JOIN [Db70].dbo.T7 va
                    	ON b.noe_va=va.C0
                    LEFT JOIN Db70.dbo.CounterVaziat c
                    	ON b.cod_vas=c.MoshtarakinId
                    LEFT OUTER JOIN [Db70].dbo.T41 k1
                    	ON b.cod_enshab=k1.C9
                    LEFT OUTER JOIN [Db70].dbo.T41 k2
                    	ON b.group1=k2.C9
                    JOIN [Db70].dbo.T5 q
                    	ON b.enshab=q.C0
                    LEFT OUTER JOIN [{dbName}].dbo.members m
                        ON b.radif=m.radif and b.town=m.town
                    WHERE b.Id=@bedBesId";//
        }
        private string GetDeleteCommand()
        {
            return $@"Delete [CustomerWarehouse].dbo.Bills
                    Where 
                    	ZoneId=@ZoneId AND
                    	CustomerNumber=@CustomerNumber AND
                    	PreviousDay=@PreviousDateJalali AND
                    	PreviousNumber=@previousNumber AND
                    	NextDay=@CurrentDateJalali AND
                    	NextNumber=@CurrentNumber ";
        }
        private string GetInsertReturnByRepairCommand(string dbName)
        {
            return $@"insert into [CustomerWarehouse].dbo.Bills 
                    select
                    	b.town ZoneId,
                    	z.C2 ZoneTitle,
                    	b.radif CustomerNumber,
                    	IIF(m.bill_id IS NOT NULL, m.bill_id,'') BillId,
                    	b.eshtrak ReadingNumber,
                    	b.pri_no PreviousNumber,
                    	b.today_no NextNumber,
                    	b.pri_date PreviousDay,
                    	b.today_date NextDay,
                    	b.date_bed RegisterDay,
                    	AbAndFazelab.dbo.PersianToMiladi(b.date_bed) RegisterDayGregorian,
                    	'' CounterStateTitle,
                    	b.cod_enshab AS [UsageId], 
                    	b.group1 [UsageId2], 
                    	k1.C1 AS [UsageTitle], 
                    	k2.C1 AS [UsageTitle2], 
                    	va.C1 AS [BranchType], 
                    	b.enshab AS [WaterDiameterId],
                    	q.C2  AS  [WaterDiameterTitle],
                    	IIF(m.sif_1 IS NULL,0,m.sif_1), 
                    	IIF(m.sif_2 IS NULL,0,m.sif_2), 
                    	IIF(m.sif_3 IS NULL,0,m.sif_3), 
                    	IIF(m.sif_4 IS NULL,0,m.sif_4), 
                    	IIF(m.sif_5 IS NULL,0,m.sif_5),   
                    	IIF(m.sif_6 IS NULL,0,m.sif_6),   
                    	IIF(m.sif_7 IS NULL,0,m.sif_7),   
                    	IIF(m.sif_8 IS NULL,0,m.sif_8),   
                    	IIF(m.fix_mas IS NOT NULL, m.fix_mas, 0) ContractCapacity,
                    	b.tedad_mas DomesticCount,
                    	b.tedad_tej CommercialCount,
                    	b.tedad_vahd OtherCount,
                    	IIF(m.Khali_s IS NOT NULL, m.khali_s,0) EmptyCount,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*b.masraf Consumption,
                    	b.modat Duration,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)* b.rate ConsumptionAverage,
                    	b.mohlat Deadline,
                    	b.jam -b.baha PreDebt,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*b.ab_baha Item1,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*b.fas_baha, 
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*abon_ab Item3, 
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*abon_fas,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*shahrdari Item5,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*ab_10 ,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*ab_20 Item7, 
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*jarime, 
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*zabresani Item9,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*zarib_d,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*zaribfasl Item11, 
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*ztadil,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*TAB_ABN_A Item13, 
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*TAB_ABN_F,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*TABS_FA Item15,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*bodjeh,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*C200 Item17,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*Avarez,
                    	IIF(type in(1,9) or (type=4 and elat=1) or (type=6 and elat =1),1,-1)*baha SumItems,
                    	0 Payable,
                    	N'برگشتی' TypeId,
                    	0,
                    	0, 
                    	0, 
                    	0,
                    	0,
                    	0,
                    	0, 
                    	0, 
                    	0,
                    	0,
                    	0, 
                    	0,
                    	0, 
                    	0,
                    	0,
                    	0,
                    	0,
                    	0,
                    	0 IsFree,
                    	IIF(m.VillageId IS NOT NULL,m.VillageId,'' ) VillageId,
                    	IIF(m.VillageName IS NOT NULL,m.VillageName,'') VillageName,
                    	b.town ZoneId2, 
                    	NULL ReadingStateTitle, 
                    	'' PayId,
                    	NULL CounterStateCode,
                    	IIF(type=4 and elat=1,3,IIF(type=4 and elat=2,4,5)) TypeCode,
                    	IIF(type=4 and elat=1,N'اصلاحات مثبت',IIF(type=4 and elat=2,N'اصلاحات منفی',N'برگشتی')) TypeTitle,
                    	rc.Id,
                    	rc.Title,
                    	b.noe_va,
                    	0 IsSettlement
                    from [{dbName}].dbo.REPAIR b
                    join Db70.dbo.T51 z
                    	on b.town=z.C0
                    JOIN [Db70].dbo.T7 va
                    	ON b.noe_va=va.C0
                    LEFT OUTER JOIN [{dbName}].dbo.members m
                    	on b.radif=m.radif and b.town=m.town
                    LEFT OUTER JOIN [Db70].dbo.T41 k1
                    	ON b.cod_enshab=k1.C9
                    LEFT OUTER JOIN [Db70].dbo.T41 k2
                    	ON b.group1=k2.C9
                    JOIN [Db70].dbo.T5 q
                    	ON b.enshab=q.C0
                    LEFT OUTER JOIN [Db70].dbo.BillReturnCause rc
                    	ON b.elat=rc.Id
                    WHERE b.id=@id";
        }

    }
}
