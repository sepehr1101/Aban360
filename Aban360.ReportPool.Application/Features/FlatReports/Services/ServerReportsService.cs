using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Contracts;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.FlatReports.Services;

public interface IServerReportsService
{
    Task FailAsync(Guid id, CancellationToken cancellationToken);
    Task<ServerReportsGetFilePathDto> GetFilePathAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<ServerReportsGetAllDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<ServerReportsGetAllDto>> GetAllByThresholdAsync(Guid userId,int threshold, CancellationToken cancellationToken);
    Task UpdateAsync(ServerReportsUpdateDto UpdateDto, CancellationToken cancellationToken);
    Task CreateAsync(ServerReportsCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ServerReportsService : IServerReportsService
{
    private readonly IServerReportsGetFilePathService _serverReportsGetFilePathService;
    private readonly IServerReportsGetAllService _serverReportsGetAllService;
    private readonly IServerReportsGetAllByThresholdService _serverReportsGetAllByThresholdService;
    private readonly IServerReportsUpdateService _serverReportsUpdateService;
    private readonly IValidator<ServerReportsUpdateDto> _updateValidator;
    private readonly IServerReportsCreateService _serverReportsCreateService;
    private readonly IValidator<ServerReportsCreateDto> _createValidator;

    public ServerReportsService(IServerReportsGetFilePathService serverReportsGetFilePathService, IServerReportsGetAllService serverReportsGetAllService, IServerReportsGetAllByThresholdService serverReportsGetAllByThresholdService, IServerReportsUpdateService serverReportsUpdateService, IValidator<ServerReportsUpdateDto> updateValidator, IServerReportsCreateService serverReportsCreateService, IValidator<ServerReportsCreateDto> createValidator)
    {
        serverReportsGetFilePathService.NotNull(nameof(serverReportsGetFilePathService));
        _serverReportsGetFilePathService = serverReportsGetFilePathService;
        serverReportsGetAllService.NotNull(nameof(serverReportsGetAllService));
        _serverReportsGetAllService = serverReportsGetAllService;
        serverReportsGetAllByThresholdService.NotNull(nameof(serverReportsGetAllByThresholdService));
        _serverReportsGetAllByThresholdService = serverReportsGetAllByThresholdService;
        serverReportsUpdateService.NotNull(nameof(serverReportsUpdateService));
        _serverReportsUpdateService = serverReportsUpdateService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        serverReportsCreateService.NotNull(nameof(serverReportsCreateService));
        _serverReportsCreateService = serverReportsCreateService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public Task FailAsync(Guid id, CancellationToken cancellationToken) => _serverReportsUpdateService.Fail(id, cancellationToken);

    public async Task<ServerReportsGetFilePathDto> GetFilePathAsync(Guid id, CancellationToken cancellationToken)
    {
        ServerReportsGetFilePathDto result = await _serverReportsGetFilePathService.Get(id);
        return result;
    }

    public async Task<IEnumerable<ServerReportsGetAllDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _serverReportsGetAllService.Get(userId);
        return result;
    }

    public async Task<IEnumerable<ServerReportsGetAllDto>> GetAllByThresholdAsync(Guid userId,int threshold, CancellationToken cancellationToken)
    {
        var result = await _serverReportsGetAllByThresholdService.Get(userId,threshold);
        return result;
    }

    public async Task UpdateAsync(ServerReportsUpdateDto UpdateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(UpdateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        await _serverReportsUpdateService.Update(UpdateDto, cancellationToken);
    }

    public async Task CreateAsync(ServerReportsCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        await _serverReportsCreateService.Create(createDto);
    }

}
