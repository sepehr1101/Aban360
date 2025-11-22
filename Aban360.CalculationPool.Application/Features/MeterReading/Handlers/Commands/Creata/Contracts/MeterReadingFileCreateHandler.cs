using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DotNetDBF;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts
{
    internal sealed class MeterReadingFileCreateHandler : IMeterReadingFileCreateHandler
    {
        private readonly IMeterReadingFileService _meterReadingFileService;
        private readonly IValidator<MeterReadingFileByFormFileCreateDto> _validator;
        public MeterReadingFileCreateHandler(
            IMeterReadingFileService meterReadingFileService,
            IValidator<MeterReadingFileByFormFileCreateDto> validator)
        {
            _meterReadingFileService = meterReadingFileService;
            _meterReadingFileService.NotNull(nameof(_meterReadingFileService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task Handle(MeterReadingFileByFormFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validation(input, cancellationToken);
            MeterReadingFileCreateDto meterReadingFileToCreate = GetMeterReadingFileCreateDto(input, appUser);
            var records = ReadDbfFile(filePath);

        }
        private async Task Validation(MeterReadingFileByFormFileCreateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private MeterReadingFileCreateDto GetMeterReadingFileCreateDto(MeterReadingFileByFormFileCreateDto input, IAppUser appUser)
        {
            return new MeterReadingFileCreateDto()
            {
                Title = input.Title,
                AgentCode = 1,//todo
                FileName = input.ReadingFile.FileName,
                ZoneId = 131211,//
                RecordCount = 1000,//
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
            };
        }
        private void ReadDb(string filePath)
        {
            var result = new List<Dictionary<string, object>>();

            FileStream stream = File.OpenRead(filePath);
            DBFReader reader = new DBFReader(stream);

            int recordCount = reader.RecordCount;
            DBFField[] fields = reader.Fields;

            object[] rowObjects;
            while ((rowObjects = reader.NextRecord()) != null)
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < fields.Length; i++)
                {
                    string fieldName = fields[i].Name;
                    object fieldValue = rowObjects[i];

                    row.Add(fieldName, fieldValue);
                }

                result.Add(row);
            }
        }

        public List<Dictionary<string, object>> ReadDbfFile(string filePath)
        {
            var result = new List<Dictionary<string, object>>();

            using (var stream = File.OpenRead(filePath))
            using (var reader = new DBFReader(stream))
            {
                int recordCount = reader.RecordCount;
                var fields = reader.Fields;

                object[] rowObjects;

                while ((rowObjects = reader.NextRecord()) != null)
                {
                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < recordCount; i++)
                    {
                        var fieldName = fields[i].Name;
                        var fieldValue = rowObjects[i];
                        row[fieldName] = fieldValue;
                    }

                    result.Add(row);
                }
            }

            return result;
        }

    }
}
