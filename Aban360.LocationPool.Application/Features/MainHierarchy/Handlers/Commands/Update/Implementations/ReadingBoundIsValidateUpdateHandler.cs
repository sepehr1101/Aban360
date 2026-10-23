using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class ReadingBoundIsValidateUpdateHandler : IReadingBoundIsValidateUpdateHandler
    {
        private readonly IReadingBoundQueryService _readingBoundQueryService;
        private readonly IReadingBlockQueryService _readingBlockQueryService;
        public ReadingBoundIsValidateUpdateHandler(
            IReadingBoundQueryService readingBoundQueryService,
            IReadingBlockQueryService readingBlockQueryService)
        {
            _readingBoundQueryService = readingBoundQueryService;
            _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));

            _readingBlockQueryService = readingBlockQueryService;
            _readingBlockQueryService.NotNull(nameof(readingBlockQueryService));
        }

        public async Task Handle(ReadingBoundIsValidUpdateDto updateDto, CancellationToken cancellationToken)
        {
            ReadingBound readingBound = await _readingBoundQueryService.Get(updateDto.Id);
            readingBound.IsValid = false;

            ICollection<ReadingBlock> readingBlocks = await _readingBlockQueryService.GetByBoundId(readingBound.Id);
            if (!readingBlocks.Any())
            {
                throw new ReadingException(ExceptionLiterals.ReadingBlockNotFound);
            }
            ICollection<ReadingBlock> readingBlockOrdered = readingBlocks.OrderBy(block => block.FromReadingNumber).ToList();
            string fromReadingNumber = readingBlockOrdered?.FirstOrDefault()?.FromReadingNumber ?? string.Empty;
            string toReadingNumber = readingBlockOrdered?.OrderByDescending(block => block.FromReadingNumber)?.FirstOrDefault()?.ToReadingNumber ?? string.Empty;
            if (string.IsNullOrWhiteSpace(fromReadingNumber) || string.IsNullOrWhiteSpace(toReadingNumber))
            {
                throw new ReadingException(ExceptionLiterals.ReadingBlockNotFound);
            }

            string perToReadingNumber = "";
            for (int i = 0; i < (readingBlockOrdered?.Count() ?? 0); i++)
            {
                ReadingBlock? block = readingBlockOrdered.ElementAt(i);

                if (fromReadingNumber.Length != block.FromReadingNumber.Length || fromReadingNumber.Length != block.ToReadingNumber.Length)
                {
                    throw new ReadingException(ExceptionLiterals.InvalidBlocksReadingNumberChars);
                }

                if (i == 0 && readingBound.FromReadingNumber != block.FromReadingNumber)
                {
                    throw new ReadingException(ExceptionLiterals.InvalidBoundAndBlockStartReadingNumber);
                }
                if (i > 0)
                {
                    if (Convert.ToInt32(block.FromReadingNumber) != Convert.ToInt32(perToReadingNumber) + 1)
                    {
                        throw new ReadingException(string.Format(ExceptionLiterals.InvalidStartReadingNumber, block.Title));
                    }
                }

                perToReadingNumber = block.ToReadingNumber;
            }

            readingBound.IsValid = true;

        }
    }
}
