using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Implementations
{
    internal sealed class VosolEnQueryService : AbstractBaseConnection, IVosolEnQueryService
    {
        public VosolEnQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<ServiceLinkPaidDataOutputDto>> Get(ServiceLinkPaidInputDto input)
        {
            string query = GetByDateQuery(GetDbName(input.ZoneId));
            IEnumerable<ServiceLinkPaidDataOutputDto> result = await _sqlReportConnection.QueryAsync<ServiceLinkPaidDataOutputDto>(query, input);
            return result;
        }
        public async Task<ServiceLinkPaidDataOutputDto> Get(ServiceLinkPaymentRemoveInputDto input)
        {
			//Atlas;
			string dbName = GetDbName(input.ZoneId);
            string query = GetByCustomerNumberQuery(dbName);
            ServiceLinkPaidDataOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ServiceLinkPaidDataOutputDto>(query, input);
			if (result == null)
			{
				throw new InvalidBillCommandException(ExceptionLiterals.NotFountVosolEn);
			}
			return result;
        }

        private string GetByDateQuery(string dbName)
        {
            return $@"Select 
						ID,
						town ZoneId,
						t51.C2 ZoneTitle,
						radif CustomerNumber,
						sh_ghabs BillId,
						pay_date PayDateJalali,
						date_bank BankDateJalali,
						date_bes RegisterDateJalali,
						serial BankCode,
						TRIM(cod_bank) BankBranchCode,
						cod3 Amount
					From [{dbName}].dbo.vosolEN 
					Join [Db70].dbo.T51 t51
						On town=t51.C0
					Where
						town=@zoneId AND
						date_bes =@PayDateJalali";
        }
        private string GetByCustomerNumberQuery(string dbName)
        {
            return $@"Select 
						ID,
						town ZoneId,
						t51.C2 ZoneTitle,
						radif CustomerNumber,
						sh_ghabs BillId,
						pay_date PayDateJalali,
						date_bank BankDateJalali,
						date_bes RegisterDateJalali,
						serial BankCode,
						cod_bank BankCode,
						cod3 Amount
					From [{dbName}].dbo.vosolEN 
					Join [Db70].dbo.T51 t51
						On town=t51.C0
					Where
						town=@ZoneId AND
						radif =@CustomerNumber AND
						ID=@Id";
        }
    }
}
