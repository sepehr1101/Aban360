using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    public sealed class TankerQueryService : AbstractBaseConnection, ITankerQueryService
    {
        const string _title = "آب تانکری";
        public TankerQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<TankerWaterHeaderOutputDto, TankerWaterDateOutputDto>> Get(TankerWaterInputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string query = GetQuery(dbName);
            IEnumerable<TankerWaterDateOutputDto> data = await _sqlReportConnection.QueryAsync<TankerWaterDateOutputDto>(query, input);
            TankerWaterHeaderOutputDto header = new(data?.Count() ?? 0, _title, data?.Sum(d => d.Amount) ?? 0);
            ReportOutput<TankerWaterHeaderOutputDto, TankerWaterDateOutputDto> result = new(_title, header, data);

            return result;
        }
        public async Task<TankerOutputDto> Get(ZoneIdAndCustomerNumber input)
        {
            string dbName = GetDbName(input.ZoneId);
            string query = GetByCustomerNumber(dbName);
            TankerOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<TankerOutputDto>(query, input);
            if (result is null)
            {
                throw new TankerException(ExceptionLiterals.InvalidCustomerNumber);
            }
            return result;
        }
        private string GetQuery(string dbName)
        {
            return $@"Select 
                    	t46.C0 RegionId,
                    	t46.C2 RegionTitle,
                    	t.town ZoneId,
                    	t51.C2 ZoneTitle,
                    	t.radif CustomerNumber,
                    	TRIM(t.name) FirstName,
                    	TRIM(t.family) Surname,
                    	TRIM(t.name)+' '+TRIM(t.family) FullName,
                    	TRIM(t.address) Address,
                    	t.barge, 
                    	t.masraf Consumption,
                    	t.baha Amount,
                    	t.date RegisterDateJalali,
                    	t.TAG_NOT_SHORB IsNotShorb,
						a.sh_ghabs BillId,
						a.pard PaymentAmount,
						a.sh_pard PaymentId,
						a.cod_bank BankCode,
						a.date_bank BankDateJalali ,
						a.pay_date PaymentDateJalali,
						a.type_pay PaymentTypeId,
						t150.C2 PaymentTypeTitle
                    From [{dbName}].dbo.tanker t
					Left Join [{dbName}].dbo.vosolab a
						ON t.radif=a.radif AND t.town=a.town
                    Join [Db70].dbo.T51 t51 
                    	ON t.town=t51.C0
                    Join [Db70].dbo.T46 t46 
                    	ON t51.C1=t46.C0
                    Left Join [Db70].dbo.T150 t150
                    	On a.type_pay Collate SQL_Latin1_General_CP1_CI_AS=t150.C1 Collate SQL_Latin1_General_CP1_CI_AS
                    Where
                        date BETWEEN @FromDateJalali AND @ToDateJalali AND
	                    t.user_hasf=0";
        }
        private string GetByCustomerNumber(string dbName)
        {
            return $@"Select 
                    	t.town ZoneId,
                    	t51.C2 ZoneTitle,
                    	t46.C0 RegionId,
                    	t46.C2 RegionTitle,
                    	t.radif CustomerNumber,
                    	TRIM(t.name) FirstName,
                    	TRIM(t.family) Surname,
                    	TRIM(t.address) Address,
                    	t.barge Barge,
                    	t.baha Amount,
                    	t.masraf Consumption,
                    	t.date RegisterDateJalali,
                    	t.del IsDeleted,
                    	t.user_hasf DeletedBy,
                    	t.date_hasf DeletedDateJalali ,
						a.sh_ghabs BillId,
						a.pard PaymentAmount,
						a.sh_pard PaymentId,
						a.cod_bank BankCode,
						a.date_bank BankDateJalali ,
						a.pay_date PaymentDateJalali,
						a.type_pay PaymentTypeId,
						t150.C2 PaymentTypeTitle
                    From [{dbName}].dbo.tanker t
					Left Join [{dbName}].dbo.vosolab a
						ON t.radif=a.radif AND t.town=a.town
                    Join [Db70].dbo.T51 t51
                    	On t.town=t51.C0
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
                    Left Join [Db70].dbo.T150 t150
                    	On a.type_pay Collate SQL_Latin1_General_CP1_CI_AS=t150.C1 Collate SQL_Latin1_General_CP1_CI_AS
                    Where t.radif=@CustomerNumber";
        }

    }
}
