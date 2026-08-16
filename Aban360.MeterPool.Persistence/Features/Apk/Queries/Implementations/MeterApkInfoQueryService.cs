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

        public async Task<IEnumerable<ApkInfoGetDto>> GetValid()
        {
            string query = GetValidQuery();
            IEnumerable<ApkInfoGetDto> result = await _sqlConnection.QueryAsync<ApkInfoGetDto>(query);
            return result;
        }
        public async Task<ApkInfoGetDto> GetLatestVersion()
        {
            string query = GetLatestVersionQuery();
            ApkInfoGetDto result = await _sqlConnection.QueryFirstOrDefaultAsync<ApkInfoGetDto>(query);
            return result;
        }
        public async Task<ApkInfoGetDto?> GetValid(short id)
        {
            string query = GetByIdQuery();
            ApkInfoGetDto? result = await _sqlConnection.QueryFirstOrDefaultAsync<ApkInfoGetDto>(query, new { id });
            return result;
        }
        public async Task<ApkInfo?> GetValid(string version)
        {
            string query = GetValidByVersionQuery();
            ApkInfo? result = await _sqlConnection.QueryFirstOrDefaultAsync<ApkInfo>(query, new { version });
            return result;
        }
        public async Task<byte[]> GetFile(short id)
        {
            string query = GetFileByIdQuery();
            byte[] result = await _sqlConnection.QueryFirstOrDefaultAsync<byte[]>(query, new { id });
            return result;
        }
   
        private string GetValidQuery()
        {
            return @"Select 
                    	Id,
                    	Name,
                        --FileContent,
                    	Version,
                    	Description,
                    	InsertedDateTime
                    From Aban360.MeterPool.ApkInfo
                    Where RemovedBy IS NULL";
        }
        private string GetLatestVersionQuery()
        {
            return @"Select Top 1
                    	Id,
                    	Name,
                    	Version,
                        FileContent,
                    	Description,
                    	InsertedDateTime
                    From Aban360.MeterPool.ApkInfo
                    Where
                        IsActive = 1 AND
                        RemovedBy IS NULL AND
                        ExpiredBy IS NULL
                    Order By version Desc";
        }
        private string GetByIdQuery()
        {
            return @"Select 
                    	Id,
                    	Name,
                    	Version,
                        FileContent,
                    	Description,
                    	InsertedDateTime
                    From Aban360.MeterPool.ApkInfo
                    Where 
                        Id=@Id AND
                        RemovedBy IS NULL AND
                        ExpiredBy IS NULL";
        }
        private string GetValidByVersionQuery()
        {
            return @"Select 
                    	Id,
                    	Name,
                    	Version,
                        FileContent,
                    	Description,
                    	InsertedDateTime,
                        RemovedBy,
                        ExpiredBy
                    From [Aban360].MeterPool.ApkInfo
                    Where 
                        Version=@Version AND
                        RemovedBy IS NULL AND
                        ExpiredBy IS NULL";
        }
        private string GetFileByIdQuery()
        {
            return @"Select FileContent
                    From Aban360.MeterPool.ApkInfo
                    Where Id=@Id";
        }
    }
}
