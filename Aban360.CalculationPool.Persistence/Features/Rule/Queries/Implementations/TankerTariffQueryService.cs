using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Implementations
{
    internal sealed class TankerTariffQueryService : AbstractBaseConnection, ITankerTariffQueryService
    {
        public TankerTariffQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<TankerTariffGetDto> Get(int zoneId)
        {
            string query = GetByZoneIdQuery();
            TankerTariffGetDto? tankerTariff = await _sqlReportConnection.QueryFirstOrDefaultAsync<TankerTariffGetDto>(query, new { zoneId });
            if (tankerTariff is null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundTankerTariffForCurrentZone);
            }

            return tankerTariff;
        }

        public async Task<IEnumerable<TankerTariffGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<TankerTariffGetDto> TankerTariff = await _sqlReportConnection.QueryAsync<TankerTariffGetDto>(query);

            return TankerTariff;
        }
        private string GetByZoneIdQuery()
        {
            return @"Select 
                    	Id,
                    	ZoneId,
                    	ZoneTitle,
                    	WaterFormula
                    From OldCalc.dbo.TankerTariff
                    Where ZoneId = @ZoneId";
        }
        private string GetAllQuery()
        {
            return @"Select 
                    	Id,
                    	ZoneId,
                    	ZoneTitle,
                    	WaterFormula
                    From OldCalc.dbo.TankerTariff";
        }
    }
}
