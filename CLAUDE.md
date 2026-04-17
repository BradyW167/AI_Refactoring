# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Phases

Current Phase: 1

### Phase Rules

- Do not refactor beyond the current phase's scope
- Focus on only one set of phase requirements at a time
- Build the program using the 'Build & Run' section below - must show 0 errors
- I will verify Game mechanic functionality manually
- Append changes to REFACTOR_LOG.md into a header with the current phase number

### 1  - Extracting Player, Enemy, and MovingPlatform Logic

#### Guidelines
- Encapsulate player movement and mechanics
- Do not move collision logic, grounded checks, or scoring to any of these classes
- Create a Player class that owns player movement, jumping, gravity, and speed
- frmMain's timer should forward keyboard input and call Player methods rather than changing movement variables directly.
- Extract enemy and moving platform classes
- Refactor duplicate movement logic into reusable classes such as Enemy and MovingPlatform. Each object should manage its own speed, direction, and update behavior.

#### New Classes
- Player
- Enemy
- MovingPlatform

### 2 - Extracting GameEngine Logic

#### Guidelines
- Move collisions and game rules into a GameEngine class.
- Such a class will coordinate collisions, scoring, grounded/platform logic, victory, and death conditions.
- The UI form should become a thinner UI shell that mainly delegates to the engine.

#### New Classes
- GameEngine

### 3 - Extracting Database Interaction and Building Data Storage

#### Guidelines
- Separate persistence and application flow
- Move database connection logic and methods to DatabaseManager class
- Move login validation, score insertion, and score retrieval into DatabaseManager class
- Create Score and User classes to store database data with OOP structure
- UI Forms should no longer contain raw SQL logic directly

#### New Classes
- DatabaseManager
- Score
- User

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
- Sprites: stored in Properties/Resources, accessed in code through Resources.resourcefilenamehere
