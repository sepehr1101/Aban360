# Aban360 project instructions

## Execution environment

- The project's primary execution environment is native Windows.
- The project directory is `D:\Projects\Aban360`.
- Use PowerShell and Windows-compatible commands and paths when working in this environment.
- Work on this project only in the native Windows checkout at `D:\Projects\Aban360`. Do not switch to Linux, WSL, a cloud checkout, or another copy for implementation, review, builds, or tests unless the user explicitly requests it.
- If Windows tools are temporarily unavailable, report that limitation and resume in the Windows checkout when available. Do not search for or use a Linux copy as a fallback, and do not ask the user to change or reconnect their project merely because the active tool environment differs.
- Check the active execution environment and project directory before running commands. If the session temporarily switches to Linux and this repository is absent, explain that the active execution environment has changed; do not conclude that the Windows project has been removed or disconnected.
- Preserve existing workspace changes when resuming work in the Windows environment.

## C# style

- Prefer explicit types for local variables, including in `foreach` and `out` declarations. Use `var` only when an explicit type is impractical or unavailable, such as with anonymous types.
