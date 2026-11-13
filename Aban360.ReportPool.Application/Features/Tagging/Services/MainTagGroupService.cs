using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Tagging.Services;

public interface IMainTagGroupService
{
    Task<MainTagGroupGetDto> GetSingleAsync(int id);
    Task<IEnumerable<MainTagGroupGetDto>> GetAllAsync();
    Task CreateAsync(MainTagGroupInsertInputDto input);
    Task UpdateAsync(MainTagGroupUpdateDto input);
    Task DeleteAsync(int id);
}

internal sealed class MainTagGroupService : AbstractBaseConnection, IMainTagGroupService
{
    private readonly IMainTagGroupQueryService _service;
    private readonly ITagGroupQueryService _tagGroupService;
    private readonly Func<IDbConnection> _connectionFactory;

    public MainTagGroupService(IMainTagGroupQueryService service, ITagGroupQueryService tagGroupService, IConfiguration configuration) : base(configuration)
    {
        service.NotNull(nameof(service));
        _service = service;
        tagGroupService.NotNull(nameof(tagGroupService));
        _tagGroupService = tagGroupService;
        _connectionFactory = () => _sqlReportConnection;
    }

    internal MainTagGroupService(IMainTagGroupQueryService service, ITagGroupQueryService tagGroupService, IConfiguration configuration, Func<IDbConnection> connectionFactory)
        : this(service, tagGroupService, configuration)
    {
        connectionFactory.NotNull(nameof(connectionFactory));
        _connectionFactory = connectionFactory;
    }

    public async Task<MainTagGroupGetDto> GetSingleAsync(int id)
    {
        MainTagGroupGetDto data = await _service.GetValid(id);
        return data;
    }

    public async Task<IEnumerable<MainTagGroupGetDto>> GetAllAsync()
    {
        IEnumerable<MainTagGroupGetDto> data = await _service.GetValid();
        return data;
    }

    public async Task CreateAsync(MainTagGroupInsertInputDto input)
    {
        MainTagGroupInsertDto insertDto = new(input.Title);
        using (IDbConnection connection = _connectionFactory())
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MainTagGroupCommandService mainTagGroupCommandService = new(connection, transaction);
                await mainTagGroupCommandService.Insert(insertDto);
                transaction.Commit();
            }
        }
    }

    public async Task UpdateAsync(MainTagGroupUpdateDto input)
    {
        using (IDbConnection connection = _connectionFactory())
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MainTagGroupCommandService mainTagGroupCommandService = new(connection, transaction);
                await mainTagGroupCommandService.Update(input);
                transaction.Commit();
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        MainTagGroupRemoveDto removeDto = new(id);
        IEnumerable<TagGroupDto> tagGroupsInfo = await _tagGroupService.GetByMainTagGroupId(id);
    
        using (IDbConnection connection = _connectionFactory())
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MainTagGroupCommandService mainTagGroupCommandService = new(connection, transaction);
                TagGroupCommandService tagGroupCommandService = new(connection, transaction);
                TagCommandService tagCommandService = new(connection, transaction);
    
                await mainTagGroupCommandService.Remove(removeDto);
                await tagGroupCommandService.DeleteByMainGroupId(id);
                await tagCommandService.DeleteByTagGroupId(tagGroupsInfo.Select(t => t.Id));
    
                transaction.Commit();
            }
        }
    }

}
