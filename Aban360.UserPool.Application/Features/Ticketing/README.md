# Ticketing application

Follows the existing UserPool feature structure: `Handlers/Commands/Create`,
`Handlers/Commands/Update`, `Handlers/Queries`, separate `Contracts` and
`Implementations`, `Handle` methods, AutoMapper profiles and FluentValidation
validators. DTOs live in UserPool.Domain. Existing application assembly scanning
registers the handlers, profiles, validators and access service.

Handlers stage changes only. The caller commits through the UserPool unit of
work, as existing UserPool controllers do. Creating a ticket stages its initial
message and attachments as one graph. The create handler returns the unique
ticket number; identity IDs are available after saving.

Pass a trusted `IAppUser` from the authenticated request, never from request JSON.
The ticket owner, assigned responder and administrator can read a ticket.
Administrators see all tickets; other users see their own tickets by default or
their assigned queue by setting `AssignedToUserId` to their own ID. Reports follow
the same scope. Only an administrator can change assignment. The assigned
responder or administrator can change status and reopen/close tickets.

New messages on closed tickets are rejected. User follow-ups set status to
InProgress; staff responses set it to Answered. Only a message's sender can edit
its text or attachments. Only the ticket owner can rate another sender's response
or edit that rating. The database unique constraint also protects against
concurrent duplicate ratings. Physical deletion is not exposed.

Files are validated by name, extension, MIME type and basic file signature.
Supported types: JPEG, PNG, GIF, WebP, MP4 and WebM. Defaults are five attachments
per message and 50 MiB per file, defined in `TicketFileCreateValidator`. Signature
checks do not fully decode media. File content DTOs use byte arrays. API controllers
set HTTP upload limits and return range-enabled file responses; direct database streaming
is not implemented. File metadata
is authorized before loading file bytes.

Smoke checks: `dotnet run --project tests/Aban360.Ticketing.SmokeTests`.
These use an in-memory database and validate mapping, registration, access scope,
message/rating rules and explicit commits. They do not exercise live SQL Server
or FILESTREAM infrastructure.
