# Return-bill comparison tests

Run:

```powershell
dotnet run --project tests/Aban360.ReturnBill.ComparisonTests/Aban360.ReturnBill.ComparisonTests.csproj
```

The baseline is a frozen copy of the working return-bill handlers after the new
minutes allocator was introduced and before structural refactoring. Only handler
namespaces were changed so baseline and production implementations can run together.
The tax calculator is also frozen because its interface is internal. `Baseline/sources.json`
records original source hashes. Do not refresh baseline files to make a failure pass.

Each case uses separate input objects and dependencies for the baseline and current
code. All serialized output fields, input mutations, dependency arguments and call order are
compared. Distinct monetary fields, fractional values, zero and large totals, negative
differences, three-bill aggregation, sewage and non-domestic cases are represented.
Full and partial preview handlers are executed; manual repair/autoBack/report mappings
are compared. Real tariff lookup and database persistence are outside this suite.
The return-minutes smoke/integration suite covers allocator transaction behavior.

This suite characterizes current behavior, including existing defects. Matching the
baseline does not prove that tariff policy or current financial calculations are correct.
Intentional behavior fixes require separately reviewed expectations. Generated dates
are compared at runtime; rerun if a comparison straddles local midnight.
