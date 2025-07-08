using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class HouseholdRegisterCommandService : AbstractBaseConnection, IHouseholdRegisterCommandService
    {
        public HouseholdRegisterCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Update(HouseholdRegisterUpdateDto updateDto, string date)
        {
            string householdUpateQueryString = GetHouseholdQuery();
            var @params = new
            {
                billId = updateDto.BillId,
                householdNumber = updateDto.HouseholdNumber,
                householdDate = updateDto.HouseholdDateJalali
            };
            var result = await _sqlReportConnection.ExecuteAsync(householdUpateQueryString, @params);
        }

        private string GetHouseholdQuery()
        {
            return @"Update [CustomerWarehouse].dbo.Clients
                     set		
                     	HouseholdDateJalali=@householdDate,
                     	FamilyCount=@householdNumber
                     Where
                            BillId=@billId AND
                        ToDayJalali IS NULL";
        }
    }
}
