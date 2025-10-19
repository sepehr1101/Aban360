using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class UnconfirmedSubscribersQueryService : AbstractBaseConnection, IUnconfirmedSubscribersQueryService
    {
        public UnconfirmedSubscribersQueryService(IConfiguration configuration)
        : base(configuration)
        {
        }

        public async Task<ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto>> GetInfo(UnconfirmedSubscribersInputDto input)
        {
            string unconfirmedSubscribersQuery = UnconfirmedSubscribersQuery();
            IEnumerable<UnconfirmedSubscribersDataOutputDto> unconfirmedSubscribersData = await _sqlReportConnection.QueryAsync<UnconfirmedSubscribersDataOutputDto>(unconfirmedSubscribersQuery, new { zoneIds = input.ZoneIds });
            UnconfirmedSubscribersHeaderOutputDto unconfirmedSubscribersHeader = new UnconfirmedSubscribersHeaderOutputDto()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = (unconfirmedSubscribersData is not null && unconfirmedSubscribersData.Any()) ? unconfirmedSubscribersData.Count() : 0,
                RecordCount = (unconfirmedSubscribersData is not null && unconfirmedSubscribersData.Any()) ? unconfirmedSubscribersData.Count() : 0,
                SumFinalAmount = unconfirmedSubscribersData.Sum(x => x.FinalAmount),
                SumPreInstallmentAmount = unconfirmedSubscribersData.Sum(x => x.PreInstallmentAmount),
                Title = ReportLiterals.UnconfirmedSubscribers
            };

            ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto> result = new
                (ReportLiterals.UnconfirmedSubscribers, unconfirmedSubscribersHeader, unconfirmedSubscribersData);

            return result;
        }

        private string UnconfirmedSubscribersQuery()
        {
            return @"Select 
                    	TRIM(d.FirstName) AS FirstName,
                    	TRIM(d.SurName) AS Surname,
                    	TRIM(d.Address) AS Address,
                    	TRIM(d.Firstname) + ' ' + TRIM(d.Surname) AS FullName,
                        d.BillId,
                    	d.ZoneId, 
                    	d.ZoneTitle,
                    	d.FinalAmount,
                    	d.PreInstallmentAmount,
                    	d.Mobile,
                    	d.ContractualCapacity,
                        d.TrackNumber,
						d.RequestDateJalali
                    From [CustomerWarehouse].dbo.DiscontinuedRequests d
                    Where
                    	d.ZoneId IN @zoneIds";
        }
    }
}
