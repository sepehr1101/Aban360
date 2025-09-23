using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    internal sealed class ZaribCCommandService : AbstractBaseConnection, IZaribCCommandService
    {
        public ZaribCCommandService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Update(ZaribCUpdateDto input)
        {
            string zaribCUpdateQueryString = GetZaribCUpdateQuery();
            var @params = new
            {
                id = input.Id,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                c = input.C,
                conditionGroup = input.ConditionGroup,
                isDeleted = input.IsDeleted,
            };
            await _sqlReportConnection.ExecuteAsync(zaribCUpdateQueryString, @params);

        }
        public async Task Create(ZaribCCreateDto input)
        {
            string zaribCCreateQueryString = GetZaribCCreateQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                c = input.C,
                conditionGroup = input.ConditionGroup,
                isDeleted = input.IsDeleted,
            };
            await _sqlReportConnection.ExecuteAsync(zaribCCreateQueryString, @params);
        }
        
        public async Task Delete(int id)
        {
            string zaribCDeleteQueryString = GetZaribCDeleteQuery();
            await _sqlReportConnection.ExecuteAsync(zaribCDeleteQueryString, new {id=id});
        }

        private string GetZaribCUpdateQuery()
        {
            return @"Update [OldCalc].dbo.Zarib_C
                    Set FromDateJalali=@fromDate, ToDateJalali=@toDate ,C=@c ,ConditionGroup=@conditionGroup , IsDeleted=@isDeleted
                    Where Id=@id";
        }

        private string GetZaribCCreateQuery()
        {
            return @"Insert Into [OldCalc].dbo.Zarib_C(FromDateJalali,ToDateJalali,C,ConditionGroup,IsDeleted)
                    Values(@fromDate,@toDate,@c,@conditionGroup,@isDeleted);";
        }
        
        private string GetZaribCDeleteQuery()
        {
            return @"Delete From [OldCalc].dbo.Zarib_C
                    Where Id=@id";
        }

    }
}
