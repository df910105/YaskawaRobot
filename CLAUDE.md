# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

`YRCC` is a C# client library for the Yaskawa High Speed Ethernet Server (a UDP-based robot control protocol) for YRC1000 (tested), DX200 and FS100 (untested). Solution `YaskawaRobot.sln` contains:
- `YRCC/` — the library (SDK-style csproj, multi-targets `net472;netstandard2.0`; depends on `System.Text.Encoding.CodePages`).
- `Example/` — a .NET Framework 4.7.2 console app that references YRCC (usage demo). No test project or linter exists.

## Commands

- Build: `dotnet build YaskawaRobot.sln` (or `dotnet build YRCC/YRCC.csproj`)
- Running `Example` requires a reachable controller (default IP `192.168.255.1`); there are no automated tests.

## Architecture

- Single public class `YHSES` (`public sealed partial class`), split across files in `YRCC/Library/`:
  - `YHSES.cs` — core: socket/UDP state, ports (robot control 10040, file control 10041), `Transmit`, connect/disconnect, error-packet synthesis.
  - `YHSES.RCCnn.cs` — one file per robot-control command group in the Yaskawa spec (RCC01 = alarm data, 0x70, etc.). Each file holds the public method(s), the private decode helpers, and the public data classes (e.g. `AlarmData`) for that command.
  - `YHSES.FCC01.cs` — file-control commands; currently commented out / untested.
- `YRCC/Packet/` — `PacketHeader`, `PacketReq` (builds request bytes), `PacketAns` (parses response).

### Command method pattern
Public methods follow `int ReadXxx(..., ref/out data, out ushort err_code)`: build a `PacketReq(division, req_id, cmd_no, inst, attr, service, data, size)`, call `Transmit(req.ToBytes(), port)`, set `err_code = ans.added_status`, decode `ans.data` only when `ans.status == ERROR_SUCCESS`, and return `ans.status`. Follow this pattern when adding commands.

### Transport notes
- `Transmit` is serialized by `SocketLock`; it connects lazily and disconnects after the call if it had to connect.
- Socket errors do not throw: they are converted via `GenerateErrorAnsPacket` into an answer with `ERROR_CONNECTION` and the socket error code as `added_status`, and `IsConnectOK` is updated.
- Text encoding: ASCII for dates/ids, UTF-8 for job/file names, and the configurable `YHSES.PendantEncoding` for pendant-language fields (alarm name, `DisplayMessage`, string variables). It must match the controller's pendant language and defaults to Big5 when the constructor's `pendantEncoding` argument is omitted. Big5 requires `CodePagesEncodingProvider`, registered in the `YHSES` static constructor (needed on netstandard/.NET Core). Timestamps use `DATE_PATTERN` (`yyyy/MM/dd HH:mm`).
- Source comments/XML docs are largely in Traditional Chinese; keep that style when editing existing files.
