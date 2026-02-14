using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Reflection.PortableExecutable;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    internal sealed class CustomerGeneralInfoQueryService : AbstractBaseConnection, ICustomerGeneralInfoQueryService
    {
        public CustomerGeneralInfoQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto>> Get(ZoneIdAndCustomerNumberOutputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string query = GetPersonalQuery(dbName);
            CustomerGeneralInfoDto customerInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerGeneralInfoDto>(query, input);
            if (customerInfo == null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.CustomerNotFound);
            }

            CustomerGeneralInfoDataDto data_v1 = GetData(customerInfo);
            CustomerGeneralInfoHeaderDto header = GetHeader(customerInfo);

            CustomerGeneralInfoDataDto data_v2 = await GetBillInfo(data_v1, input, dbName);
            CustomerGeneralInfoDataDto data_v3 = await GetPaymentInfo(data_v2, input, dbName);
            IEnumerable<CustomerGeneralInfoDataDto> data_v4 = [await GetMeterChangeInfo(data_v3, input, dbName)];

            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> result = new(ReportLiterals.CustomerGeneralInfo, header, data_v4);

            return result;
        }
        private async Task<CustomerGeneralInfoDataDto> GetBillInfo(CustomerGeneralInfoDataDto customerInfo, ZoneIdAndCustomerNumberOutputDto input, string dbName)
        {
            string query = GetBillInfo(dbName);
            CustomerGeneralBillInfoDto billInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerGeneralBillInfoDto>(query, input);
            if (billInfo == null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);
            }
            customerInfo.CounterStateCode = billInfo.CounterStateCode;
            customerInfo.LatestMeterNumber = billInfo.LatestMeterNumber;
            customerInfo.LatestMeterReading = billInfo.LatestMeterReading;
            customerInfo.UsageStatusTitle = billInfo.UsageStatusTitle;

            return customerInfo;
        }
        private async Task<CustomerGeneralInfoDataDto> GetMeterChangeInfo(CustomerGeneralInfoDataDto customerInfo, ZoneIdAndCustomerNumberOutputDto input, string dbName)
        {
            string query = GetMeterChangeInfo(dbName);
            CustomerGeneralMeterChangeInfoDto? latestMeterChange = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerGeneralMeterChangeInfoDto>(query, input);
            customerInfo.MeterChangeDateJalali = latestMeterChange?.MeterChangeDateJalali;

            return customerInfo;
        }
        private async Task<CustomerGeneralInfoDataDto> GetPaymentInfo(CustomerGeneralInfoDataDto customerInfo, ZoneIdAndCustomerNumberOutputDto input, string dbName)
        {
            string query = GetPaymentInfo(dbName);
            CustomerGeneralPaymentInfoDto latestMeterChange = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerGeneralPaymentInfoDto>(query, input);
            customerInfo.LatestPaymentDateJalali = latestMeterChange.LatestPaymentDateJalali;

            return customerInfo;
        }

        private CustomerGeneralInfoDataDto GetData(CustomerGeneralInfoDto input)
        {
            return new CustomerGeneralInfoDataDto()
            {
                RegionTitle = input.RegionTitle,
                ZoneTitle = input.ZoneTitle,
                PostalCode = input.PostalCode,
                N = input.N,
                E = input.E,
                Address = input.Address,

                TotalUnit = input.TotalUnit,
                EmptyUnit = input.EmptyUnit,
                HouseholdNumber = input.HouseholdNumber,

                WaterDebtAmount = input.WaterDebtAmount,
                SewageDebtAmount = input.SewageDebtAmount,
                LatestPaymentDateJalali = input.LatestPaymentDateJalali,

                CounterStateCode = input.CounterStateCode,
                LatestMeterNumber = input.LatestMeterNumber,
                MeterLife = input.MeterLife,
                BodySerial = input.BodySerial,
                MeterChangeDateJalali = input.MeterChangeDateJalali,
                LatestMeterReading = input.LatestMeterReading,
                UsageStatusTitle = input.UsageStatusTitle,
                CommonSiphon = input.CommonSiphon,

            };
        }
        private CustomerGeneralInfoHeaderDto GetHeader(CustomerGeneralInfoDto input)
        {
            return new CustomerGeneralInfoHeaderDto()
            {
                FirstName = input.FirstName,
                Surname = input.Surname,
                FullName = input.FullName,
                NationalCode = input.NationalCode,
                MobileNumber = input.MobileNumber,
                ReadingNumber = input.ReadingNumber,
                BillId = input.BillId,
                UsageTitle = input.UsageTitle,
                ContractualCapacity = input.ContractualCapacity,
                MeterDiameterId = input.MeterDiameterId,
                MeterDiameterTitle=input.MeterDiameterTitle,        
                MainSiphon = input.MainSiphon,
                BranchTypeTitle = input.BranchTypeTitle,
                DiscountType = input.DiscountType,
                WaterRequestDateJalali = input.WaterRequestDateJalali,
                WaterInstallationDateJalali = input.WaterInstallationDateJalali,
                SewageRequestDateJalali = input.SewageRequestDateJalali,
                SewageInstallationDateJalali = input.SewageInstallationDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.CustomerGeneralInfo
            };
        }

        private string GetPersonalQuery(string dbName)
        {
            return @$"Select 
						TRIM(m.name) FirstName,
						TRIM(m.family) Surname,
						(TRIM(m.name)+' '+TRIM(m.family)) as FullName,
						TRIM(m.MELI_COD) as NationalCode,
						TRIM(m.MOBILE) as MobileNumber,
						m.eshtrak as ReadingNumber,
						m.bill_id as BillId,
						t41.C1 as UsageTitle,
						m.fix_mas as ContractualCapacity,
						m.enshab as MeterDiameterId,
						t5.C2 as MeterDiameterTitle,
						Case When m.sif_1>0 Then N'قطر 100'
							 When m.sif_2>0 Then N'قطر 125'
							 When m.sif_3>0 Then N'قطر 150'
							 When m.sif_4>0 Then N'قطر 200'
							 When m.sif_5>0 Then N'قطر 5'
							 When m.sif_6>0 Then N'قطر 6'
							 When m.sif_7>0 Then N'قطر 7'
							 When m.sif_8>0 Then N'قطر 8'
							 Else N'ندارد'
						End as MainSiphon,

						m.noe_va as BranchTypeId,
						t7.C1 as BranchTypeTitle,
						0 as DiscountType,
						'-' as WaterRequestDateJalali,
						m.inst_ab as WaterInstallationDateJalali,
						'-'  as SewageRequestDateJalali,
						m.inst_fas as SewageInstallationDateJalali,
					
						t46.C2 as RegionTitle,
						t51.C2 as ZoneTitle,
						m.POST_COD as PostalCode,
						TRIM(m.address) as Address,

						m.tedad_mas+tedad_tej+tedad_vahd as TotalUnit,
						m.Khali_s as EmptyUnit,
						m.ted_khane as HouseholdNumber,

						TRIM(m.serial_co) as BodySerial,
					    m.sif_mosh_1 as CommonSiphon,
						m.bed_bes as WaterDebtAmount
					
					From [{dbName}].dbo.members m
					Left Join [Db70].dbo.T51 t51 
						ON m.town=t51.C0
					Left Join [Db70].dbo.t46 t46
						ON t51.C1=t46.C0
					Left join [Db70] .dbo.T7 t7
						ON m.noe_va=t7.C0
					Left join Db70.dbo.T5 t5
						ON m.enshab=t5.C0
					Left Join [Db70].dbo.T41 t41
						ON m.cod_enshab=t41.C0
					Where 
						town=@zoneId AND
						radif=@customerNumber";
        }
        private string GetBillInfo(string dbName)
        {
            return @$"Select Top 1
						b.cod_vas as CounterStateCode,
						b.today_no as LatestMeterNumber,
						b.today_date as LatestMeterReading,
						'-' as UsageStatusTitle
					From [{dbName}].dbo.bed_bes b
					Where 
						b.town=@zoneId AND
						b.radif=@customerNumber
					Order by b.date_bed DESC";
        }
        private string GetMeterChangeInfo(string dbName)
        {
            return $@"Select Top 1
						t.taviz_date as MeterChangeDateJalali
					From [{dbName}].dbo.taviz	t
					Where 
						t.town=@zoneId AND
						t.radif=@customerNumber
					Order by t.date_sabt DESC";
        }
        private string GetPaymentInfo(string dbName)
        {
            return @$"Select Top 1
                    	date_sabt as LatestPaymentDateJalali
                    From [{dbName}].dbo.vosolab
                    Where 
						town=@zoneId AND
						radif=@customerNumber
                    Order By date_sabt DESC";
        }
    }
}
