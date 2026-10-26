# Ticketing persistence

- `Commands/Contracts` and `Commands/Implementations`: stage inserts and scalar updates for tickets, messages, attachments and ratings. Commit the complete operation through `IUnitOfWork.SaveChangesAsync` in the application layer. Commands do not save independently.
- `Queries/Contracts` and `Queries/Implementations`: ticket search/paging, message history/paging, attachment metadata/content, ratings, fixed statuses and aggregate reporting. Existing assembly scanning registers these services automatically.
- Single ticket, message and rating lookups return tracked entities for command handlers; list, metadata, content and report queries do not track entities. Missing single records return `null`.
- Search dates are UTC: `FromDate` is inclusive and `ToDateExclusive` is exclusive. Page numbers start at 1; page size is 1–100. Report filters use ticket creation time; ratings belong to the filtered tickets regardless of rating date. An unrated result has `AverageScore = null`.
- Attachment metadata queries project only metadata columns. `GetContent` explicitly loads `FileContent` into a byte array; this is not a streaming video API. FILESTREAM storage is configured by the migration, not by EF-generated DDL.
- Authorization, allowed status transitions, updating ticket activity timestamps, response eligibility for ratings, text-or-attachment validation, file validation and size limits belong in the application layer before calling these persistence services. Do not expose persistence entities directly as API responses.
- Status rows are seeded by the migration and intentionally have no command service. Physical deletion is not exposed; tickets are closed through status updates.

Verification: `dotnet run --project tests/Aban360.Ticketing.SmokeTests`. This checks EF mappings, dependency injection and service behavior using an in-memory store. It does not validate SQL Server FILESTREAM infrastructure or execute migrations.
