# AutoReading smoke checks

Run from the repository root:

```powershell
dotnet run --project tests/Aban360.AutoReading.SmokeTests
```

Optionally pass the original 18-reading JSON sample as the first argument:

```powershell
dotnet run --project tests/Aban360.AutoReading.SmokeTests -- "C:/path/to/sample.json"
```

The checks use an in-memory HTTP transport. They exercise the POST/body contract with exactly three input properties, string-only bill IDs, the supported bill restriction and exact error message, fixed internal meter parameters, existing MeterPool DI registration, Persian-date validation, upstream query encoding, report deserialization, empty/malformed responses, upstream errors, timeout/cancellation, and the existing ESB token refresh handler. They do not contact ESB or a database. The optional original sample additionally checks all 18 records and exact counter differences; device data and credentials are not committed as fixtures.
