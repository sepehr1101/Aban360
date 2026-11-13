# Next application service consolidation candidates

Inventory checked on 2026-11-13 after the four ticketing services. Execution update: all 19 approved groups have now been consolidated (93 original handlers). DynamicReport was explicitly excluded and remains pending. Controller consolidation and handler consolidation are separate: a controller may already be merged while its handlers remain separate.

The table records the original 20-group proposal and its original 99 handler implementations. Counts include complementary queries in the same feature, not only CRUD. Same-named handlers in other modules are excluded. Every listed group except DynamicReport is complete.

| Order | Capability | Module | Handlers |
|---:|---|---|---:|
| 1 | CounterState | MeterPool | 5 |
| 2 | IndividualTagDefinition | ClaimPool | 5 |
| 3 | WaterMeterTagDefinition | ClaimPool | 5 |
| 4 | EquipmentBrokerZone | InstallationPool | 5 |
| 5 | SewageEquipmentBrokerZone | InstallationPool | 5 |
| 6 | CompanyServiceOffering | CalculationPool | 5 |
| 7 | TariffConstant | CalculationPool | 6 |
| 8 | UserWorkday | ClaimPool | 5 |
| 9 | RequestTracking | ClaimPool | 5 |
| 10 | Individual | ClaimPool | 5 |
| 11 | Siphon | ClaimPool | 5 |
| 12 | Guild | ClaimPool | 5 |
| 13 | WaterResource | ClaimPool | 5 |
| 14 | RequestIndividual | ClaimPool | 3 |
| 15 | RequestSiphon | ClaimPool | 3 |
| 16 | RequestWaterMeter | ClaimPool | 3 |
| 17 | DynamicReport | ReportPool | 6 |
| 18 | DocumentCategory | BlobPool | 6 |
| 19 | DocumentType | BlobPool | 5 |
| 20 | ReadingBound | LocationPool | 7 |

ReadingBound includes interval validation, not just CRUD. DocumentCategory includes GetByBillId and both document groups handle uploaded icons. DynamicReport includes template and master queries. TariffConstant includes its dictionary query. RequestTracking is the Draft feature, distinct from the protected TrackingFlowGetHandler. CounterState here is MeterPool's Management feature.

Direct SQL/transaction workflows such as UsageGroup1/2/3, ClientDiscount and SmsStateTemplate, token security workflows and access-tree/hierarchy features need separate prioritization; they were not ranked solely by method length.
