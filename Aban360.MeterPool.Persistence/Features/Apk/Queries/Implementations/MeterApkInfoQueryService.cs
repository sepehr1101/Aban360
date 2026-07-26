using Aban360.Common.Db.Dapper;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Persistence.Features.Apk.Queries.Implementations
{
    internal sealed class MeterApkInfoQueryService : AbstractBaseConnection, IMeterApkInfoQueryService
    {
        public MeterApkInfoQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<ApkInfoGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<ApkInfoGetDto> result = await _sqlConnection.QueryAsync<ApkInfoGetDto>(query);
            return result;
        }
        public async Task<ApkInfoGetDto> GetLatest()
        {
            string query = GetLatestQuery();
            ApkInfoGetDto result = await _sqlConnection.QueryFirstOrDefaultAsync<ApkInfoGetDto>(query);
            return result;
        }
        public async Task<ApkInfoGetDto?> Get(short id)
        {
            string query = GetByIdQuery();
            ApkInfoGetDto? result = await _sqlConnection.QueryFirstOrDefaultAsync<ApkInfoGetDto>(query, new { id });
            return result;
        }
        public async Task<ApkInfoGetDto?> Get(string version)
        {
            string query = GetByVersionQuery();
            ApkInfoGetDto? result = await _sqlConnection.QueryFirstOrDefaultAsync<ApkInfoGetDto>(query, new { version });
            return result;
        }
        public async Task<byte[]> GetFile(short id)
        {
            string query = GetFileByIdQuery();
            byte[] result = await _sqlConnection.QueryFirstOrDefaultAsync<byte[]>(query, new { id });
            return result;
        }
   
        private string GetQuery()
        {
            return @"Select 
                    	Id,
                    	Name,
                        FileContent,
                    	Version,
                    	Description,
                    	InsertedBy,
                    	InsertedDateTime,
                    	RemovedBy,
                    	RemovedDateTime,
                    	ExpiredBy,
                    	ExpiredDateTime
                    From Aban360.MeterPool.ApkInfo";
        }
        private string GetLatestQuery()
        {
            return @"Select Top 1
                    	Id,
                    	Name,
                    	Version,
                        FileContent,
                    	Description,
                    	InsertedBy,
                    	InsertedDateTime,
                    	RemovedBy,
                    	RemovedDateTime,
                    	ExpiredBy,
                    	ExpiredDateTime
                    From Aban360.MeterPool.ApkInfo
                    Where
                        RemovedBy IS NULL AND
                        ExpiredBy IS NULL
                    Order By InsertedDateTime Desc";
        }
        private string GetByIdQuery()
        {
            return @"Select 
                    	Id,
                    	Name,
                    	Version,
                        FileContent,
                    	Description,
                    	InsertedBy,
                    	InsertedDateTime,
                    	RemovedBy,
                    	RemovedDateTime,
                    	ExpiredBy,
                    	ExpiredDateTime
                    From Aban360.MeterPool.ApkInfo
                    Where Id=@Id";
        }
        private string GetByVersionQuery()
        {
            return @"Select 
                    	Id,
                    	Name,
                    	Version,
                        FileContent,
                    	Description,
                    	InsertedBy,
                    	InsertedDateTime,
                    	RemovedBy,
                    	RemovedDateTime,
                    	ExpiredBy,
                    	ExpiredDateTime
                    From Aban360.MeterPool.ApkInfo
                    Where Version=@Version";
        }
        private string GetFileByIdQuery()
        {
            return @"Select FileContent
                    From Aban360.MeterPool.ApkInfo
                    Where Id=@Id";
        }
    }
}
