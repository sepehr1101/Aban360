# Compact handler consolidation

This wave replaces 43 handlers with 14 scoped services and migrates 24 consumers. Selection considered cohesion, class size and dependency count; line count alone is not a guarantee against a code smell. Larger workflows, direct SQL operations, financial calculation engines, and the protected `TrackingFlowGetHandler` were excluded.

Class lines include the implementation declaration and closing brace, excluding imports and the public interface. The largest class is 113 lines. The implementation constructor has at most seven distinct dependencies.

| Service | Original handlers | Class lines | Dependencies |
| --- | ---: | ---: | ---: |
| Tariff | 8 | 106 | 6 |
| TariffTests | 3 | 109 | 3 |
| Invoice | 2 | 99 | 7 |
| MeterReadingFileReports | 2 | 74 | 4 |
| MeterFlowLookup | 2 | 113 | 6 |
| SaleDictionaries | 3 | 33 | 3 |
| RequestUsage | 5 | 75 | 5 |
| AssessmentLookups | 4 | 88 | 3 |
| AssessmentApk | 2 | 39 | 0 |
| LocationLookup | 2 | 26 | 1 |
| BillReturnLookups | 4 | 83 | 4 |
| BillReturnRepair | 2 | 21 | 1 |
| HttpLogQueries | 2 | 68 | 2 |
| MaaherInvoiceQueries | 2 | 27 | 2 |

## Preserved behavior

The original method bodies, validation, mapping, arithmetic, calls and await order are preserved with declared identifier changes. Routes, HTTP verbs, action attributes, authorization metadata, input/output signatures, persistence and transaction boundaries are preserved. Invoice support records retain their original public namespace. No change was made to the interval tariff engine or location tree factory.

Frozen source was captured from the working tree immediately before this wave, including earlier uncommitted work. `CompactBaseline/operations.json` lists every original implementation, contract, source hash, destination and rename. `consumers.json` records each migrated consumer.

## Verification

- API and Bridge API builds: successful, existing compiler/analyzer warnings remain.
- `CriticalServiceStructure.RegressionTests -- --next28`: 625 checks for this wave and 801 checks for the preceding 28-service wave. Checks include complete method syntax, constructors, fields, original snapshot hashes, public signatures, consumer metadata, factory and test substitute equivalence.
- `MergedControllers.SmokeTests -- --compact`: 1,107 checks across 659 before/after scenarios, 46 successful application fixtures, and 70 consumer methods. All 14 services resolve with scoped lifetimes. Exercises dependency failures, cancellation, pending asynchronous completion, validation rejection and empty inputs.
- Invoice-specific assertions check amount conversion, item amounts, entity relationships, installments, meter history, write order and the existing caller-controlled commit boundary. APK tests read a disposable test file and compare bytes and name.
- `MergedControllers.SmokeTests -- --next28`: 204 checks / 80 before/after scenarios, plus 27 review checks for the earlier wave.
- Critical controller regression: 1,639 checks including actual JWT middleware/MVC binding with isolated dependencies.
- Batch141: 342 checks. Batch160Bridge: 234 checks. Batch160Api: 185 checks.
- Existing controller consolidation: 3,869 checks. Batch110: 2,424 checks. Batch132: 1,079 checks. Batch151: 552 checks.
- Existing HTTP error-log tests: 27 checks; existing HTTP report tests pass date validation, filters, paging and cancellation against the actual new service.

The broad legacy run still has the previously documented CollectBills contract fixture mismatches: Batch136 expects the earlier action count; MergedControllers expects the earlier combined GET/POST declaration. This wave does not change `CollectBillsController` (also unchanged against Git HEAD). The historical fixtures are retained rather than recaptured to hide these failures. See `service-consolidation-71-100.md` for the preceding review.

Run focused checks from the repository root:

```powershell
dotnet run --project tests/Aban360.CriticalServiceStructure.RegressionTests -- --next28
dotnet run --project tests/Aban360.MergedControllers.SmokeTests -- --compact
dotnet run --project tests/Aban360.MergedControllers.SmokeTests -- --next28
```

The focused suites use isolated dependencies. No live database integration, production writes, real SMS, Hangfire execution or IIS deployment was performed. These checks support equivalence for the consolidation; they do not certify the pre-existing business rules.

## Repeat review — 2026-11-20

Reviewed the current native Windows checkout at `D:\Projects\Aban360`. Rebuilt API, Bridge API and the merged-controller test project successfully. Re-ran all 625 compact and 801 preceding-wave source checks, the compact runtime suite (1,107 checks / 659 scenarios), and the preceding-wave runtime suite (204 checks / 80 scenarios plus 27 review checks): all passed. No production references to the removed compact handlers/contracts remain; no Domain, Persistence or protected TrackingFlow changes are present in the working-tree diff. Manual review covered invoice amounts/entity links/write order, tariff consumers and meter-reading dependency bindings. No new consolidation defect was identified. No business code was changed during this review; the Windows-only execution instruction in `AGENTS.md` was clarified.
