﻿using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record WaterMeterCommandDto
    {
       // public int Id { get; set; }

        public string? InstallationLocation { get; set; }

        public string? BodySerial { get; set; }

        public string? InstallationDate { get; set; }

        public string? ProductDate { get; set; }

        public string? GuaranteeDate { get; set; }

        public short MeterDiameterId { get; set; }

        public short MeterProducerId { get; set; }

        public short MeterTypeId { get; set; }

        public short MeterMaterialId { get; set; }

        public short MeterUseTypeId { get; set; }

        public UseStateEnum UseStateId { get; set; }

        public SubscriptionTypeEnum SubscriptionTypeId { get; set; }


        public string? ReadingNumber { get; set; }

        public int CustomerNumber { get; set; }

        public int SealNumber { get; set; }

        public string BillId { get; set; } = null!;

        public int EstateId { get; set; }

        public Guid UserId { get; set; }
    }
}
