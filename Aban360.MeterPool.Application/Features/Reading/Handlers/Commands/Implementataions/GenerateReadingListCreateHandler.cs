using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Aban360.MeterPool.Domain.Constants;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Commands;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using YamlDotNet.Core.Tokens;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Implementataions
{
    //public interface IGenerateReadingListCreateHandler
    //{
    //    Task Handle(ReadingCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    //}
    //internal sealed class GenerateReadingListCreateHandler : AbstractBaseConnection, IGenerateReadingListCreateHandler
    //{
    //    private readonly IHttpContextAccessor _contextAccessor;
    //    private readonly ICommonZoneService _commonZoneService;
    //    private readonly IReadingBoundQueryService _readingBoundQueryService;
    //    private readonly IReadingBlockQeryService _readingBlockQeryService;
    //    private readonly IT51QueryService _zoneQueryService;
    //    private readonly IReadingCustomerDetailInfoQueryService _readingCustomerDetailInfoQueryService;
    //    private readonly IUserSearchByRoleIdHandler _userSearchByRoleIdHandler;
    //    private readonly IValidator<ReadingCreateDto> _validator;
    //    private static DateTime _currentDateTime = DateTime.Now;
    //    private static string _currentDateJalali = _currentDateTime.ToShortPersianDateString();
    //    private static int _createPathStepId = (int)ReadingFlowStepEnum.CreatePath;
    //    public GenerateReadingListCreateHandler(
    //        IHttpContextAccessor contextAccessor,
    //        ICommonZoneService commonZoneService,
    //        IReadingBoundQueryService readingBoundQueryService,
    //        IReadingBlockQeryService readingBlockQeryService,
    //        IT51QueryService zoneQueryService,
    //        IReadingCustomerDetailInfoQueryService readingCustomerDetailInfoQueryService,
    //        IUserSearchByRoleIdHandler userSearchByRoleIdHandler,
    //        IValidator<ReadingCreateDto> validator,
    //        IConfiguration configuration)
    //            : base(configuration)
    //    {
    //        _contextAccessor = contextAccessor;
    //        _contextAccessor.NotNull(nameof(contextAccessor));

    //        _commonZoneService = commonZoneService;
    //        _commonZoneService.NotNull(nameof(commonZoneService));

    //        _readingBoundQueryService = readingBoundQueryService;
    //        _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));

    //        _readingBlockQeryService = readingBlockQeryService;
    //        _readingBlockQeryService.NotNull(nameof(readingBlockQeryService));

    //        _zoneQueryService = zoneQueryService;
    //        _zoneQueryService.NotNull(nameof(zoneQueryService));

    //        _readingCustomerDetailInfoQueryService = readingCustomerDetailInfoQueryService;
    //        _readingCustomerDetailInfoQueryService.NotNull(nameof(readingCustomerDetailInfoQueryService));

    //        _userSearchByRoleIdHandler = userSearchByRoleIdHandler;
    //        _userSearchByRoleIdHandler.NotNull(nameof(userSearchByRoleIdHandler));

    //        _validator = validator;
    //        _validator.NotNull(nameof(validator));
    //    }

    //    public async Task Handle(ReadingCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    //    {
    //        await Validate(inputDto, appUser, cancellationToken);

    //    }
    //    private async Task ExecSql(ReadingCreateDto inputDto, IAppUser appUser)
    //    {
    //        BoundInsertDto newBoundDto = await GetBoundInsert(inputDto, appUser);//ToDo:set CustomerCount 
    //        IEnumerable<BlockInsertDto> newBlocksDto = await GetBlocksInsert(inputDto, newBoundDto);//ToDo: set CustomerCount, BoundId 
    //        IEnumerable<TrackingInsertDto> newTrackingsDto;
    //        IEnumerable<DetailInsertDto> newDetailDto;

    //        using (IDbConnection connection = _sqlReportConnection)
    //        {
    //            if (connection.State != ConnectionState.Open)
    //            {
    //                connection.Open();
    //            }
    //            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
    //            {
    //                BoundCommandService boundCommandService = new(connection, transaction);
    //                BlockCommandService blockCommandService = new(connection, transaction);
    //                TrackingCommandService trackingCommandService = new(connection, transaction);
    //                DetailCommandService detailCommandService = new(connection, transaction);
    //                //OpLog?

    //                await boundCommandService.Insert(newBoundDto);//ToDo: update CustomerCount
    //                await blockCommandService.Insert(newBlocksDto);//ToDo: update CustomerCount
    //                //await trackingCommandService.Insert()

    //                transaction.Commit();
    //            }
    //        }
    //    }
    //    private async Task<IEnumerable<BlockInsertDto>> GetBlocksInsert(ReadingCreateDto inputDto, BoundInsertDto newBoundDto)
    //    {
    //        int stepOrder = 0;
    //        //GetUserByReaderRole
    //        ICollection<ReadingBlock> blocksInfo = await _readingBlockQeryService.Get(inputDto.ReadingBlockInfo.Select(r => r.ReadingBlockId));
    //        ICollection<BlockInsertDto> newBlockList = new List<BlockInsertDto>();
    //        ICollection<TrackingInsertDto> newTrackingList = new List<TrackingInsertDto>();

    //        foreach (var item in blocksInfo)
    //        {
    //            IEnumerable<CustomerInfoByBillsAndChangeGetDto> customersInfo = await _readingCustomerDetailInfoQueryService.Get(newBoundDto.ZoneId, item.FromReadingNumber, item.ToReadingNumber);
    //            BlockInsertDto newBlockInfo = GetNewBlock(newBoundDto.Id, customersInfo?.Count() ?? 0, ++stepOrder, item);
    //            foreach (var perCustomer in customersInfo)
    //            {
    //                TrackingInsertDto newTrackingInfo = new()
    //                {
    //                    BlockId = newBlockInfo.Id,
    //                    TrackNumber = 0,
    //                    TbReadingProgramId = Guid.Empty,
    //                    ZoneId = newBoundDto.ZoneId,
    //                    ZoneTitle = newBoundDto.ZoneTitle,
    //                    FlowStepId = _createPathStepId,
    //                    FromReadingNumber = item.FromReadingNumber,
    //                    ToReadingNumber = item.ToReadingNumber,
    //                    InsertBy = newBoundDto.InsertBy,
    //                    InsertDateJalali = _currentDateJalali,
    //                    InsertDateTime = _currentDateTime,

    //                    CounterReaderCode =,
    //                    CounterReaderId =,
    //                    CounterReaderName =,
    //                    CounterReaderMobile =,
    //                    IsBazdid =,
    //                    OverallCustomerCount =,
    //                    ItemCustomerCount =,

    //                };
    //            }


    //            newBlockList.Add(newBlockInfo);
    //        }
    //    }
    //    private async Task<BoundInsertDto> GetBoundInsert(ReadingCreateDto inputDto, IAppUser appUser)
    //    {
    //        ReadingBound readingBoundInfo = await _readingBoundQueryService.Get(inputDto.ReadingBoundId);
    //        NumericDictionary zoneInfo = await _zoneQueryService.Get(readingBoundInfo.ZoneId, true);
    //        return new BoundInsertDto()
    //        {
    //            ReadingBoundId = readingBoundInfo.Id,
    //            ReadingBoundTitle = readingBoundInfo.Title,
    //            ZoneId = zoneInfo.Id,
    //            ZoneTitle = zoneInfo.Title,
    //            FromReadingNumber = readingBoundInfo.FromReadingNumber,
    //            ToReadingNumber = readingBoundInfo.ToReadingNumber,
    //            IsValidated = true,
    //            BlockCount = inputDto.ReadingBlockInfo?.Count() ?? 0,
    //            CustomerCount = 0,//compeleteAfter
    //            Year = inputDto.Year,
    //            ReadingPeriodTypeId = inputDto.ReadingPeriodTypeId,
    //            ReadingPeriodId = inputDto.ReadingPeriodId,
    //            InsertBy = appUser.UserId,
    //            InsertDateTime = _currentDateTime,
    //        };
    //    }
    //    private async Task Validate(ReadingCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    //    {
    //        await InputValidate(inputDto, cancellationToken);
    //        await ZoneValidate(inputDto, appUser);
    //    }
    //    private async Task InputValidate(ReadingCreateDto inputDto, CancellationToken cancellationToken)
    //    {
    //        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
    //        if (!validationResult.IsValid)
    //        {
    //            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
    //            throw new CustomValidationException(message);
    //        }
    //    }
    //    private async Task ZoneValidate(ReadingCreateDto inputDto, IAppUser appUser)
    //    {
    //        ReadingBound readingBoundInfo = await _readingBoundQueryService.Get(inputDto.ReadingBoundId);
    //        ICollection<ReadingBlock> readingBlocksInfo = await _readingBlockQeryService.Get(inputDto.ReadingBlockInfo.Select(r => r.ReadingBlockId));
    //        if (readingBlocksInfo.Where(b => b.ReadingBoundId != readingBoundInfo.Id).Any() &&
    //            (readingBlocksInfo?.Count() ?? 0) != (inputDto?.ReadingBlockInfo?.Count() ?? 0))
    //        {
    //            throw new ReadingException(ExceptionLiterals.InvalidReadingBoundWithReadingBlock);
    //        }
    //    }
    //    private BlockInsertDto GetNewBlock(Guid newBoundId, int customerCount, int stepOrder, ReadingBlock readingBlockInfo)
    //    {
    //        return new BlockInsertDto()
    //        {
    //            BoundId = newBoundId,
    //            ReadingBlockId = readingBlockInfo.Id,
    //            ReadingBlockTitle = readingBlockInfo.Title,
    //            FromReadingNumber = readingBlockInfo.FromReadingNumber,
    //            ToReadingNumber = readingBlockInfo.ToReadingNumber,
    //            CustomerCount = customerCount,
    //            OrderDigit = stepOrder,
    //            OrderPersian = $"{stepOrder}-{stepOrder.ToPersianNumbers()}",
    //        };
    //        //        }
    //        //        private TrackingInsertDto GetNewTracking()
    //        //        {
    //        //            return new TrackingInsertDto()
    //        //            {
    //        //BlockId
    //        //TrackNumber
    //        //TbReadingProgramId
    //        //ZoneId
    //        //ZoneTitle
    //        //FlowStepId
    //        //FromReadingNumber
    //        //ToReadingNumber
    //        //InsertBy
    //        //InsertDateJalali
    //        //InsertDateTime

    //        //CounterReaderCode
    //        //CounterReaderId
    //        //CounterReaderName
    //        //CounterReaderMobile
    //        //IsBazdid
    //        //OverallCustomerCount
    //        //ItemCustomerCount

    //        //            };
    //    }
    //}
}