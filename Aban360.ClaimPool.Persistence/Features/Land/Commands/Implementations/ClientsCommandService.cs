using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public sealed class ClientsCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ClientsCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task InsertByArchMemId(int id, string dbName)
        {
            string command = GetInsertByArchMemCommand(dbName);
			await _connection.ExecuteAsync(command, new { id }, transaction: _transaction);
        }
        public async Task UpdateToDayJalali(ZoneIdCustomerNumber input, string CurrentDateJalali)
        {
            string command = GetUpdateToDayJalaliCommand();
            int rowEffectCount = await _connection.ExecuteAsync(command, new { input.ZoneId, input.CustomerNumber, CurrentDateJalali }, _transaction);
            if (rowEffectCount <= 0)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidUpdateToDayJalali);
            }
        }
        private string GetInsertByArchMemCommand(string dbName)
        {
            return $@"INSERT INTO [CustomerWarehouse].dbo.Clients 
					(
					   [ZoneId]--1
					  ,[ZoneTitle]
				      ,[IsVillage]
				      ,[CustomerNumber]
				      ,[BillId]
				      ,[ReadingNumber]
				      ,[FirstName]
				      ,[SureName]
				      ,[FatherName]
					  ,[PhoneNo] --10
					  ,[MobileNo]
					  ,[PostalCode]
					  ,[NationalId]
				      ,[UsageId]
					  ,[UsageId2]
					  ,[UsageTitle]
					  ,[UsageTitle2]
					  ,[BranchType]
				      ,[WaterDiameterId]
					  ,[WaterDiameterTitle] --20
					  ,[Siphon100]
				      ,[Siphon125]
				      ,[Siphon150]
					  ,[Siphon200]
					  ,[Siphon5]
				      ,[Siphon6]
				      ,[Siphon7]
				      ,[Siphon8] 
				      ,[DomesticCount]--30
				      ,[CommercialCount]
				      ,[OtherCount]
				      ,[EmptyCount]
				      ,[FamilyCount]
				      ,[RegisterationJalaliDate]
				      ,[FieldArea]
				      ,[ConstructedArea]
				      ,[DomesticArea]
				      ,[CommercialArea]
				      ,[WaterRequestDate] --40
				      ,[SewageRequestDate]
				      ,[WaterInstallDate]
				      ,[SewageInstallDate]
				      ,[HasWater]
				      ,[HasSewage]
				      ,[Address]
				      ,[IsGovermental]
				      ,[DeletionStateId]
					  ,[DeletionStateTitle]--50
				      ,[UsageStateId]      
				      ,[ContractCapacity]
				      ,[VillageId]
				      ,[VillageName]
				      ,[X]
				      ,[Y]
					  ,IsSpecial
					  ,DiscountTypeId
					  ,DiscountTypeTitle
					  ,MeterSerialBody
					  ,HasCommonSiphon
					  ,TempRowNumber
					  ,RegisterDayJalali
					  ,FromDayJalali
					  ,ToDayJalali
					  ,HouseholdDateJalali
					  ,GuildId
					  ,GuildTitle
					  ,IsNonPermanent
					  ,MainSiphonTitle
					  ,OldCustomerNumber
					  ,OldBillId
					  ,WaterRegisterDateJalali
					  ,SewageRegisterDateJalali
					  ,LocalId
					  ,BlockCode
					  ,PhysicalWaterInstallDateJalali
					  ,PhysicalSewageInstallDateJalali)
				SELECT
					  m.town  AS  [ZoneId], --1
					  z.C2  AS  [ZoneTitle], 
					  IIF(m.town>140000,1,0) AS IsVillage,
				      m.radif  AS  [CustomerNumber], 
				      old_m.bill_id  AS  [BillId], 
				      TRIM(m.eshtrak)  AS  [ReadingNumber],
				      m.[name] AS  [FirstName], 
				      m.family  AS  [SureName], 
				      m.father_nam  AS  [FatherName], 
					  m.PHONE_NO [PhoneNo],  --10
					  IIF(old_m.MOBILE IS NULL, '', old_m.MOBILE)  AS  [MobileNo],
					  m.POST_COD  AS  [PostalCode], 
					  m.MELI_COD  AS  [NationalId],
				      m.cod_enshab  AS  [UsageId], 
					  m.group1 [UsageId2], 
					  k1.C1  AS  [UsageTitle], 
					  k2.C1  AS  [UsageTitle2], 
					  va.C1  AS  [BranchType], 
				      m.enshab  AS  [WaterDiameterId],
					  q.C2  AS  [WaterDiameterTitle], 
				      m.sif_1  AS  [Siphon100], 
					  m.sif_2  AS  [Siphon125], 
					  m.sif_3  AS  [Siphon150], 
					  m.sif_4  AS  [Siphon200], 
					  m.sif_5  AS  [Siphon5],   
					  m.sif_6  AS  [Siphon6],   
					  m.sif_7  AS  [Siphon7],   
					  m.sif_8  AS  [Siphon8],   
				      m.tedad_mas  AS  [DomesticCount], 
				      m.tedad_tej  AS  [CommercialCount], 
				      m.tedad_vahd  AS  [OtherCount], 
				      m.Khali_s  AS  [EmptyCount], 
				      m.ted_khane  AS  [FamilyCount], 
				      m.date_sabt  AS  [RegisterationJalaliDate], 
				      m.arse  AS  [FieldArea], 
				      m.aian  AS  [ConstructedArea], 
				      m.aian_mas  AS  [DomesticArea], 
				      m.aian_tej  AS  [CommercialArea], 
				      m.ask_ab  AS  [WaterRequestDate], 
				      m.ask_fas  AS  [SewageRequestDate], 
				      m.g_inst_ab  AS  [WaterInstallDate], 
				      m.G_inst_fas  AS  [SewageInstallDate], 
				      IIF(m.g_inst_ab>'1330/01/01' and m.n_ab>0,1,0)  AS  [HasWater], 
				      IIF(m.inst_fas>'1330/01/01' and m.n_faz>0,1,0)  AS  [HasSewage], 
				      m.address  AS  [Address], 
				      m.edareh_k  AS  [IsGovermental], 
				      m.hasf  AS  [DeletionStateId], 
					  del.Title  AS  [DeletionStateTitle], 
				      m.noe_va  AS  [UsageStateId], 
				      m.fix_mas  AS  [ContractCapacity], 
				      old_m.VillageId  AS  [VillageId], 
				      old_m.VillageName  AS  [VillageName], 
				      X  AS  [X], 
				      Y [Y],
					  m.edareh_k IsSpecial,
					  IIF(m.noe_va=6 or m.noe_va=7,m.noe_va,0) DiscountTypeId,
					  IIF(m.noe_va=6,N'بهزیستی', IIF(m.noe_va=7, N'کمیته امداد', N'ندارد') ) DiscountTypeTitle,
					  m.serial_co MeterSerialBody,
					  CAST(m.sif_mosh_1 as bit) HasCommonSiphon,
					  0,
					  m.date_roz,
					  m.date_roz,
					  NULL,
					  m.date_khane,
					  m.Senf,
					  g.Title,
					  m.MOJAVZ,
					  m.master_sif,
					  m.oRadif,
					  old_m.old_bill_id,
					  m.G_inst_ab,
					  m.G_inst_fas,	  
					  m.id,
					  NULL BlockCode,
					  m.inst_ab,
					  m.inst_fas
					FROM [{dbName}].dbo.arch_mem m
					LEFT OUTER JOIN [{dbName}].dbo.members old_m
						ON m.town=old_m.town AND m.radif=old_m.radif
					LEFT OUTER JOIN [Db70].dbo.T41 k1
						ON m.cod_enshab=k1.C9
					LEFT OUTER JOIN [Db70].dbo.T41 k2
						ON m.group1=k2.C9
					JOIN [Db70].dbo.T7 va
						ON m.noe_va=va.C0
					JOIN [Db70].dbo.T5 q
						ON m.enshab=q.C0
					JOIN [Db70].dbo.T51 z
						ON m.town=z.C0
					JOIN [Db70].dbo.DeletionState del 
						ON m.hasf=del.Id 
					LEFT OUTER JOIN [CounterReadingTest01].dbo.Guild g
						ON m.Senf=g.moshtarakinId AND g.IsActive=1
					WHERE old_m.bill_id<>'' AND m.date_roz>='1401/01/01' AND m.id=@id
					ORDER BY m.town, m.radif, m.id";
        }
        private string GetUpdateToDayJalaliCommand()
        {
            return $@"Update CustomerWarehouse.dbo.Clients
					Set ToDayJalali=@CurrentDateJalali
					Where 
						ZoneId=@zoneId AND
						CustomerNumber=@CustomerNumber AND
						ToDayJalali IS NULL";
        }
    }
}
