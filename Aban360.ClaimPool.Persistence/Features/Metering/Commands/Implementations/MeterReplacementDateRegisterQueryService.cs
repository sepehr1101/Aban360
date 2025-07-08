using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    internal sealed class MeterReplacementDateRegisterQueryService : AbstractBaseConnection, IMeterReplacementDateRegisterQueryService
    {
        public MeterReplacementDateRegisterQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task Update(MeterReplacementDateRegisterUpdateDto input)
        {
            string meterReplacementDateRegisterQuery = GetMeterReplacementDateRegisterQuery();
            var @params = new { };
            var result = await _sqlReportConnection.ExecuteAsync(meterReplacementDateRegisterQuery, @params);
        }


        private string GetMeterReplacementDateRegisterQuery()
        {
            return @"";
        }

    }
}
