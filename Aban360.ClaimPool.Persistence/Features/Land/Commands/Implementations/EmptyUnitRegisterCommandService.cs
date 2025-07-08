using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class EmptyUnitRegisterCommandService : AbstractBaseConnection, IEmptyUnitRegisterCommandService
    {
        public EmptyUnitRegisterCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Update(EmptyUnitRegisterUpdateDto updateDto, string date)
        {
            string EmptyUnitUpateQueryString = GetEmptyUnitQuery();
            var @params = new
            {
                billId = updateDto.BillId,
            };
            var result = await _sqlReportConnection.ExecuteAsync(EmptyUnitUpateQueryString, @params);
        }

        private string GetEmptyUnitQuery()
        {
            return @"Update [CustomerWarehouse].dbo.Clients
                    set EmptyCount=2
                    Where 
                        BillId=@billId AND
                        ToDayJalali IS NULL ";
        }
    }
}
