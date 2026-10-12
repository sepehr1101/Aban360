using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Implementataions
{
    public interface IReadingCreateHandle
    {
        Task Handle(ReadingCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
    internal sealed class ReadingCreateHandle : AbstractBaseConnection, IReadingCreateHandle
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IReadingBlockQeryService _readingBlockQeryService;
        private readonly IReadingBoundQueryService _readingBoundQueryService;
        private readonly IValidator<ReadingCreateDto> _validator;
        public ReadingCreateHandle(
            IHttpContextAccessor contextAccessor,
            ICommonZoneService commonZoneService,
            IReadingBlockQeryService readingBlockQeryService,
            IReadingBoundQueryService readingBoundQueryService,
            IValidator<ReadingCreateDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _readingBlockQeryService = readingBlockQeryService;
            _readingBlockQeryService.NotNull(nameof(readingBlockQeryService));

            _readingBoundQueryService = readingBoundQueryService;
            _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(ReadingCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, appUser, cancellationToken);
        }
        private async Task Validate(ReadingCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);

        }
        private async Task InputValidate(ReadingCreateDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task ZoneValidate(ReadingCreateDto inputDto, IAppUser appUser)
        {
            ReadingBound readingBoundInfo = await _readingBoundQueryService.Get(inputDto.ReadingBoundId);
            ICollection<ReadingBlock> readingBlocksInfo = await _readingBlockQeryService.Get(inputDto.ReadingBlockIds);
            if (readingBlocksInfo.Where(b => b.ReadingBoundId != readingBoundInfo.Id).Any())
            {
                //throw exception
            }
        }
    }
}
