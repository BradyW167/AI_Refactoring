# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```bash
dotnet build
dotnet run
dotnet build --configuration Release
```

No automated test suite exists. The project targets `net8.0-windows` and requires Windows to run (WinForms dependency).

## Architecture

A Windows Forms platformer game (Donkey Kong–style) with user auth and persistent score tracking, built for SE361 (Software Engineering course).

**Application flow**: `Program.cs` → `frmLogin` → `frmMain` (gameplay) → `frmScores` (leaderboard)

### Forms

- **frmLogin** — SQL auth against `Users` table; launches `frmMain` on success
- **frmMain** — Core game loop (40ms timer ticks = 25 fps); handles player movement/gravity, 2 enemies on platforms, 20 collectible coins, a door exit trigger, and audio playback; saves score to DB on game end
- **frmScores** — Leaderboard via DataGridView sorted by score DESC; Play Again relaunches `frmMain`, Logout returns to `frmLogin`

### Database

SQL Server LocalDB (`JumpGameDB.mdf`). Two tables:
- `Users(username, password)` — authentication
- `Scores(username, score, time, date)` — game results

Each form manages its own `SqlConnection`. All queries use parameterized statements. Connection string uses LocalDB with integrated security (no credentials in code).

### Game Loop (frmMain)

- Timer: `tmrGame` fires every 40ms
- Player starts at (10, 525); gravity applied each tick when airborne
- 9 platforms (some move vertically); 2 enemies at 6px/8px per tick
- Collision detection covers: platforms, enemies (death), coins (score), door (next level)
- Audio: looped theme music, coin/death/game-over sound effects via `System.Media.SoundPlayer`
