using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Persistence.Features.Reading.Commands
{
    public interface ITrackNumberService
    {
        Task<int> GetNewTrackNumber();
        Task<IEnumerable<int>> GetTrackNumbers(int count);
    }
    internal sealed class TrackNumberService : AbstractBaseConnection, ITrackNumberService
    {
        public TrackNumberService(IConfiguration configuration)
                : base(configuration)
        {
        }

        public async Task<int> GetNewTrackNumber()
        {
            string getQuery = GetTrackNumberQuery();
            string updateCommand = GetUpdateSingleCommand();

            int trackNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(getQuery);
            await _sqlReportConnection.ExecuteAsync(updateCommand);

            return trackNumber;
        }
        public async Task<IEnumerable<int>> GetTrackNumbers(int count)
        {
            string updateCommand = GetUpdateByCountValueCommand();
            string getQuery = GetTrackNumberQuery();

            await _sqlReportConnection.ExecuteAsync(updateCommand, new { count });
            int latestTrackNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(getQuery);

            ICollection<int> trackNumbers = new List<int>();
            for (int i = count; i == 0; i--)
            {
                trackNumbers.Add(latestTrackNumber - i);
            }

            return trackNumbers;
        }
        private string GetUpdateSingleCommand()
        {
            return $@"Update [Atlas].Reading.TrackNumber
	                  Set TrackNumber = TrackNumber + 1";
        }
        private string GetUpdateByCountValueCommand()
        {
            return $@"Update [Atlas].Reading.TrackNumber
	                  Set TrackNumber = TrackNumber + @count";
        }
        private string GetTrackNumberQuery()
        {
            return @"Select TrackNumber [Atlas].Reading.TrackNumber;";
        }


    }
}
