using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts;
using Aban360.MeterPool.Domain.Constants;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using Aban360.MeterPool.Persistence.Features.Reading.Commands;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Implementataions
{
    internal sealed class GenerateReadingListCreateHandler : AbstractBaseConnection, IGenerateReadingListCreateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IReadingBoundQueryService _readingBoundQueryService;
        private readonly IReadingBlockQeryService _readingBlockQeryService;
        private readonly IT51QueryService _zoneQueryService;
        private readonly IReadingCustomerDetailInfoQueryService _readingCustomerDetailInfoQueryService;
        private readonly IUserRoleQueryService _userRoleQueryService;
        private readonly ITrackNumberService _trackNumberService;
        private readonly IReadingPeriodTypeQueryService _readingPeriodTypeQueryService;
        private readonly IReadingPeriodQueryService _readingPeriodQueryService;
        private readonly IValidator<GenerateReadingListInputDto> _validator;
        private static DateTime _currentDateTime = DateTime.Now;
        private static string _currentDateJalali = _currentDateTime.ToShortPersianDateString();
        private static int _createPathStepId = (int)ReadingFlowStepEnum.CreatePath;
        public GenerateReadingListCreateHandler(
            IHttpContextAccessor contextAccessor,
            ICommonZoneService commonZoneService,
            IReadingBoundQueryService readingBoundQueryService,
            IReadingBlockQeryService readingBlockQeryService,
            IT51QueryService zoneQueryService,
            IReadingCustomerDetailInfoQueryService readingCustomerDetailInfoQueryService,
            IUserRoleQueryService userRoleQueryService,
            ITrackNumberService trackNumberService,
            IReadingPeriodTypeQueryService readingPeriodTypeQueryService,
            IReadingPeriodQueryService readingPeriodQueryService,
            IValidator<GenerateReadingListInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _readingBoundQueryService = readingBoundQueryService;
            _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));

            _readingBlockQeryService = readingBlockQeryService;
            _readingBlockQeryService.NotNull(nameof(readingBlockQeryService));

            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _readingCustomerDetailInfoQueryService = readingCustomerDetailInfoQueryService;
            _readingCustomerDetailInfoQueryService.NotNull(nameof(readingCustomerDetailInfoQueryService));

            _userRoleQueryService = userRoleQueryService;
            _userRoleQueryService.NotNull(nameof(userRoleQueryService));

            _trackNumberService = trackNumberService;
            _trackNumberService.NotNull(nameof(trackNumberService));

            _readingPeriodQueryService = readingPeriodQueryService;
            _readingPeriodQueryService.NotNull(nameof(readingPeriodQueryService));

            _readingPeriodTypeQueryService = readingPeriodTypeQueryService;
            _readingPeriodTypeQueryService.NotNull(nameof(readingPeriodTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<GenerateReadingListOutputDto> Handle(GenerateReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, appUser, cancellationToken);
            var (newBoundDto, newBlockList, newTrackingList, newDetailList) = await GetBlocksInsert(inputDto, appUser);
            string opLogText = string.Format(OpLogLiterals.MeterReadingInsertOpLog, newBoundDto.Id);
            await ExecSql(newBoundDto, newBlockList, newTrackingList, newDetailList, opLogText, appUser);

            GenerateReadingListOutputDto result = await GetResult(inputDto, newBoundDto, newBlockList?.Count() ?? 0);
            return result;
        }
        private async Task ExecSql(BoundInsertDto newBoundDto, IEnumerable<BlockInsertDto> newBlockList, IEnumerable<TrackingInsertDto> newTrackingList, IEnumerable<DetailInsertDto> newDetailList, string opLogText, IAppUser appUser)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BoundCommandService boundCommandService = new(connection, transaction);
                    BlockCommandService blockCommandService = new(connection, transaction);
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    DetailCommandService detailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await boundCommandService.Insert(newBoundDto);
                    await blockCommandService.Insert(newBlockList);
                    await trackingCommandService.Insert(newTrackingList);
                    await detailCommandService.Insert(newDetailList);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task<GenerateReadingListOutputDto> GetResult(GenerateReadingListInputDto inputDto, BoundInsertDto newBoundDto, int blockCount)
        {
            return new GenerateReadingListOutputDto()
            {
                ZoneTitle = newBoundDto.ZoneTitle,
                BlockCount = blockCount,
                Year = inputDto.Year,
                ReadingPeriodTypeTitle = (await _readingPeriodTypeQueryService.Get((short)inputDto.ReadingPeriodTypeId)).Title,
                ReadingPeriodTitle = (await _readingPeriodQueryService.Get((short)inputDto.ReadingPeriodId)).Title,
                CustomerCount = newBoundDto.CustomerCount,
            };
        }
        private async Task<(BoundInsertDto, IEnumerable<BlockInsertDto>, IEnumerable<TrackingInsertDto>, IEnumerable<DetailInsertDto>)> GetBlocksInsert(GenerateReadingListInputDto inputDto, IAppUser appUser)//, BoundInsertDto newBoundDto)
        {
            int stepOrder = 0;
            int overallCustomerCount = 0;

            BoundInsertDto newBoundDto = await GetBoundInsert(inputDto, Guid.NewGuid(), appUser);
            ICollection<ReadingBlock> blocksInfo = await _readingBlockQeryService.Get(inputDto.ReadingBlockInfo.Select(r => r.ReadingBlockId));
            ICollection<BlockInsertDto> newBlockList = new List<BlockInsertDto>();
            ICollection<TrackingInsertDto> newTrackingList = new List<TrackingInsertDto>();
            ICollection<DetailInsertDto> newDetailList = new List<DetailInsertDto>();

            foreach (var item in blocksInfo)//block List
            {
                IEnumerable<CustomerInfoByBillsAndChangeGetDto> customersInfo = await _readingCustomerDetailInfoQueryService.Get(newBoundDto.ZoneId, item.FromReadingNumber, item.ToReadingNumber);
                int customerCount = customersInfo?.Count() ?? 0;
                overallCustomerCount += customerCount;
                BlockInsertDto newBlockInfo = GetNewBlock(newBoundDto.Id, customerCount, ++stepOrder, item);
                ICollection<UserQueryDto> readersInfo = await ReadersValidateAngGet(inputDto.ReadingBlockInfo, newBoundDto.ZoneId);
                PerReadingBlockInfoDto perReadingBlockInfo = inputDto.ReadingBlockInfo.Where(p => p.ReadingBlockId == item.Id).FirstOrDefault();
                UserQueryDto currentReaderInfo = readersInfo.Where(r => r.Username == perReadingBlockInfo.ReaderCode.ToString()).FirstOrDefault();
                int trackNumber = await _trackNumberService.GetNewTrackNumber();

                TrackingInsertDto newTrackingInfo = GetTrackingInsert(newBoundDto, newBlockInfo, item, currentReaderInfo, trackNumber, customerCount);//todo: completer overalCustomerCount
                foreach (var perCustomer in customersInfo)//Detail List
                {
                    DetailInsertDto newDetailInfo = GetDetail(perCustomer, newTrackingInfo);
                    newDetailList.Add(newDetailInfo);
                }

                newBlockList.Add(newBlockInfo);
                newTrackingList.Add(newTrackingInfo);
            }
            newBoundDto.CustomerCount = overallCustomerCount;
            foreach (var item in newTrackingList)
            {
                item.OverallCustomerCount = overallCustomerCount;
            }

            return (newBoundDto, newBlockList, newTrackingList, newDetailList);
        }
        private async Task<BoundInsertDto> GetBoundInsert(GenerateReadingListInputDto inputDto, Guid Id, IAppUser appUser)
        {
            ReadingBound readingBoundInfo = await _readingBoundQueryService.Get(inputDto.ReadingBoundId);
            NumericDictionary zoneInfo = await _zoneQueryService.Get(readingBoundInfo.ZoneId, true);
            return new BoundInsertDto()
            {
                Id = Id,
                ReadingBoundId = readingBoundInfo.Id,
                ReadingBoundTitle = readingBoundInfo.Title,
                ZoneId = zoneInfo.Id,
                ZoneTitle = zoneInfo.Title,
                FromReadingNumber = readingBoundInfo.FromReadingNumber,
                ToReadingNumber = readingBoundInfo.ToReadingNumber,
                IsValidated = true,
                BlockCount = inputDto.ReadingBlockInfo?.Count() ?? 0,
                CustomerCount = 0,//compeleteAfter
                Year = inputDto.Year,
                ReadingPeriodTypeId = inputDto.ReadingPeriodTypeId,
                ReadingPeriodId = inputDto.ReadingPeriodId,
                InsertBy = appUser.UserId,
                InsertDateTime = _currentDateTime,
            };
        }
        private async Task Validate(GenerateReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            await ZoneValidate(inputDto, appUser);
        }
        private TrackingInsertDto GetTrackingInsert(BoundInsertDto newBoundDto, BlockInsertDto newBlockInfo, ReadingBlock readingBlock, UserQueryDto currentReaderInfo, int trackNumber, int customerCount)
        {
            return new TrackingInsertDto()
            {
                BlockId = newBlockInfo.Id,
                TrackNumber = trackNumber,
                TbReadingProgramId = Guid.Empty,
                ZoneId = newBoundDto.ZoneId,
                ZoneTitle = newBoundDto.ZoneTitle,
                FlowStepId = _createPathStepId,
                FromReadingNumber = readingBlock.FromReadingNumber,
                ToReadingNumber = readingBlock.ToReadingNumber,
                InsertBy = newBoundDto.InsertBy,
                InsertDateJalali = _currentDateJalali,
                InsertDateTime = _currentDateTime,

                CounterReaderCode = Convert.ToInt32(currentReaderInfo.Username),
                CounterReaderId = currentReaderInfo.Id,
                CounterReaderName = currentReaderInfo.DisplayName,
                CounterReaderMobile = currentReaderInfo.Mobile,
                IsBazdid = false,
                OverallCustomerCount = 0,
                ItemCustomerCount = customerCount,
            };
        }
        private async Task<ICollection<UserQueryDto>> ReadersValidateAngGet(IEnumerable<PerReadingBlockInfoDto> selectedBlocksInfo, int zoneId)
        {
            ICollection<UserQueryDto> readerUserList = await _userRoleQueryService.GetByDefaultZone(ReportLiterals.ReadingRoleId, zoneId);
            ICollection<UserQueryDto> currentReaderInfo = new List<UserQueryDto>();
            foreach (var reader in selectedBlocksInfo)
            {
                UserQueryDto? readerInfo = readerUserList.Where(user => user.Username == reader.ReaderCode.ToString()).FirstOrDefault();
                if (readerInfo is null)
                {
                    throw new ReadingException(ExceptionLiterals.InvalidReadingBoundWithReaderZoneId);
                }
                currentReaderInfo.Add(readerInfo);
            }

            return currentReaderInfo;
        }
        private async Task InputValidate(GenerateReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task ZoneValidate(GenerateReadingListInputDto inputDto, IAppUser appUser)
        {
            ReadingBound readingBoundInfo = await _readingBoundQueryService.Get(inputDto.ReadingBoundId);
            ICollection<ReadingBlock> readingBlocksInfo = await _readingBlockQeryService.Get(inputDto.ReadingBlockInfo.Select(r => r.ReadingBlockId));
            if (readingBlocksInfo.Where(b => b.ReadingBoundId != readingBoundInfo.Id).Any() &&
                (readingBlocksInfo?.Count() ?? 0) != (inputDto?.ReadingBlockInfo?.Count() ?? 0))
            {
                throw new ReadingException(ExceptionLiterals.InvalidReadingBoundWithReadingBlock);
            }
        }
        private BlockInsertDto GetNewBlock(Guid newBoundId, int customerCount, int stepOrder, ReadingBlock readingBlockInfo)
        {
            return new BlockInsertDto()
            {
                BoundId = newBoundId,
                ReadingBlockId = readingBlockInfo.Id,
                ReadingBlockTitle = readingBlockInfo.Title,
                FromReadingNumber = readingBlockInfo.FromReadingNumber,
                ToReadingNumber = readingBlockInfo.ToReadingNumber,
                CustomerCount = customerCount,
                OrderDigit = stepOrder,
                OrderPersian = $"{stepOrder}-{stepOrder.NumberToText(language:Language.Persian)}",
            };
        }
        private DetailInsertDto GetDetail(CustomerInfoByBillsAndChangeGetDto customerInfo, TrackingInsertDto newTrackingInfo)
        {
            return new DetailInsertDto()
            {
                TrackingId = newTrackingInfo.Id,
                TrackNumber = newTrackingInfo.TrackNumber,
                BillId = customerInfo.BillId,
                CustomerNumber = customerInfo.CustomerNumber,
                ReadingNumber = customerInfo.ReadingNumber,
                FirstName = customerInfo.FirstName,
                SureName = customerInfo.Surname,
                Address = customerInfo.Address,
                Plaque = customerInfo.Plaque,
                UsageId = customerInfo.UsageId,
                DomesticUnit = customerInfo.DomesticUnit,
                CommercialUnit = customerInfo.CommercialUnit,
                OtherUnit = customerInfo.OtherUnit,
                MeterDiameterId = customerInfo.MeterDiameterId,
                SiphonDiameterId = customerInfo.MainSiphon,
                PostalCode = customerInfo.PostalCode,
                PreNumber = customerInfo.BillNumber,
                PreDateJalali = customerInfo.BillDateJalali,
                PreDateTime = null,
                PreConsumptionAverage = customerInfo.BillConsumption,
                PreCounterStateCode = customerInfo.BillCounterStateCode,
                BodySerial = customerInfo.BodySerial,
                MeterInstallationDateJalali = customerInfo.MeterInstallationDateJalali,
                ChangeDateJalali = customerInfo.ChangeDateJalali,
                ChangeNumber = customerInfo.ChangeNumber,
                ContractualCapacity = customerInfo.ContractualCapacity,
                MobileNumber = customerInfo.MobileNumber,
                DeletionStateId = customerInfo.DeletionStateId,
                BranchTypeId = customerInfo.BranchTypeId,
            };
        }
    }
}