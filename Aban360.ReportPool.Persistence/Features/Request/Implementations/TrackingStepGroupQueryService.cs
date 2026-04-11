using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;
using Aban360.ReportPool.Persistence.Features.Request.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Request.Implementations
{
    internal sealed class TrackingStepGroupQueryService : AbstractBaseConnection, ITrackingStepGroupQueryService
    {
        public TrackingStepGroupQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<TrackingStepHeaderOutputDto, TrackingStepGroupDataOutputDto>> Get(TrackingInputDto input)
        {
            var (conditionGrouping, title) = GetGroupingQuery(input.IsZoneGroup);
            string _title = ReportLiterals.Tracking + title;
            string query = GetQuery(conditionGrouping);

            IEnumerable<TrackingStepGroupDataOutputDto> data = await _sqlReportConnection.QueryAsync<TrackingStepGroupDataOutputDto>(query, input);
            TrackingStepHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                Title = _title,
                RecordCount = data?.Count() ?? 0,
                RequestCount=data?.Sum(d=>d.Count) ?? 0,
            };

            return new ReportOutput<TrackingStepHeaderOutputDto, TrackingStepGroupDataOutputDto>(_title, header, data);
        }
        private (string, string) GetGroupingQuery(bool isZoneGroup)
        {
            string zoneGrouping = "t51";
            string regionGrouping = "t46";
            return isZoneGroup ? (zoneGrouping, ReportLiterals.ByZone) : (regionGrouping, ReportLiterals.ByRegion);
        }
        private string GetQuery(string conditionGrouping)
        {
            return @$"Select 
                    	MAX(t46.C0) RegionId,
                    	MAX(t46.C2) RegionTitle,
                    	MAX(t51.C0) ZoneId,
                    	MAX(t51.C2) ZoneTitle,
                    	t.Status StatusId,
                    	MAX(s.SummaryDescription) StatusTitle,
                    	COUNT(1) [Count]
                    From AbAndFazelab.dbo.Tracking t
                    Join Db70.dbo.T51 t51 
                    	ON t.ZoneID=t51.C0
                    Join Db70.dbo.T46 t46 
                    	ON t51.C1=t46.C0
                    Join AbAndFazelab.dbo.Status s
                    	On t.Status=s.StatusID
                    Where	
                    	(t.DateTimeJalali Between @FromDateJalali AND @ToDateJalali) AND
                    	t.ZoneID IN @ZoneIds
                    Group By t.Status,{conditionGrouping}.C0
                    Order By t.Status,{conditionGrouping}.C0 ";
        }
    }
}
